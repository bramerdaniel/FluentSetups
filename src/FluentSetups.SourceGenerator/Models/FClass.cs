// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FClass.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;

   using Microsoft.CodeAnalysis;

   /// <summary>Model describing a setup class</summary>
   internal class FClass : IFluentSetupClass
   {
      #region Constants and Fields

      private readonly List<FField> fields = new List<FField>(5);

      private readonly List<IFluentMethod> methods = new List<IFluentMethod>(5);

      private readonly List<FProperty> properties = new List<FProperty>(5);

      #endregion

      #region Constructors and Destructors

      internal FClass(FluentGeneratorContext context, INamedTypeSymbol classSymbol, AttributeData fluentSetupAttribute)
      {
         Context = context;
         ClassSymbol = classSymbol ?? throw new ArgumentNullException(nameof(classSymbol));
         FluentSetupAttribute = fluentSetupAttribute ?? throw new ArgumentNullException(nameof(fluentSetupAttribute));

         ClassName = classSymbol.Name;
         ContainingNamespace = ComputeNamespace();
         Modifier = ComputeModifier(classSymbol);
         EntryClassName = fluentSetupAttribute.GetSetupEntryClassName();
         EntryClassNamespace = fluentSetupAttribute.GetSetupEntryNameSpace() ?? classSymbol.ContainingAssembly.MetadataName;

         FillMembers();
      }

      #endregion

      #region IFluentSetupClass Members

      public string ClassName { get; }

      public INamedTypeSymbol ClassSymbol { get; }

      public string Modifier { get; }

      public string EntryMethod => ComputeSetupMethod();

      #endregion

      #region Public Properties

      public string ContainingNamespace { get; }

      public FluentGeneratorContext Context { get; }

      public string EntryClassName { get; }

      public string EntryClassNamespace { get; }

      public IReadOnlyList<FField> Fields => fields;

      /// <summary>Gets the fluent setup attribute that was placed onto the input source.</summary>
      public AttributeData FluentSetupAttribute { get; }

      public GeneratorMode GenerationMode
      {
         get
         {
            if (ClassSymbol.ContainingType != null)
               return GeneratorMode.None;

            if (ClassSymbol.DeclaringSyntaxReferences.Length > 1)
               return GeneratorMode.EntryMethod;

            return GeneratorMode.SetupAndEntryMethod;
         }
      }

      public bool IsPublic => ClassSymbol.DeclaredAccessibility == Accessibility.Public;

      public IReadOnlyList<IFluentMethod> Methods => methods;

      public IReadOnlyList<FProperty> Properties => properties;

      public FTarget Target { get; private set; }

      public TargetGenerationMode TargetMode { get; set; }

      public string TargetTypeName => Target?.TypeName;

      public string TargetTypeNamespace { get; set; }

      #endregion

      #region Public Methods and Operators

      public string ToCode()
      {
         var sourceBuilder = new StringBuilder();
         OpenNamespace(sourceBuilder);

         AppendRequiredUsingDirectives(sourceBuilder);

         if (Fields.Any(x => x.HasDefaultValue))
            sourceBuilder.AppendLine("#pragma warning disable CS0414");

         OpenClass(sourceBuilder);
         GenerateSetupMembers(sourceBuilder);
         CloseClass(sourceBuilder);

         CloseNamespace(sourceBuilder);

         return sourceBuilder.ToString();
      }

      #endregion

      #region Methods

      private static void CloseClass(StringBuilder sourceBuilder)
      {
         sourceBuilder.AppendLine("}");
      }

      private static string ComputeModifier(ITypeSymbol typeSymbol)
      {
         if (typeSymbol != null)
         {
            switch (typeSymbol.DeclaredAccessibility)
            {
               case Accessibility.NotApplicable:
               case Accessibility.Private:
                  return "private";
               case Accessibility.ProtectedAndInternal:
                  return "protected internal";
               case Accessibility.Protected:
                  return "protected";
               case Accessibility.Internal:
                  return "internal";
               case Accessibility.ProtectedOrInternal:
                  return "internal";
               case Accessibility.Public:
                  return "public";
            }
         }

         return "internal";
      }

      private static bool TargetCreationPossible(FClass classModel)
      {
         if (classModel.TargetMode == TargetGenerationMode.Disabled)
            return false;

         return !string.IsNullOrWhiteSpace(classModel.TargetTypeName);
      }

      private bool AddField(FField field)
      {
         if (field == null)
            throw new ArgumentNullException(nameof(field));

         if (fields.Contains(field))
            return false;

         fields.Add(field);
         return true;
      }

      private void AddMember(ISymbol member)
      {
         switch (member)
         {
            case IFieldSymbol fieldSymbol:
            {
               var field = new FField(fieldSymbol, FindMemberAttribute(fieldSymbol));
               fields.Add(field);
               break;
            }
            case IPropertySymbol propertySymbol:
            {
               var property = new FProperty(propertySymbol, FindMemberAttribute(propertySymbol));
               properties.Add(property);
               break;
            }
            case IMethodSymbol methodSymbol:
            {
               methods.Add(CreateMethod(methodSymbol));
               break;
            }
         }
      }

      private bool AddMethod(IFluentMethod method)
      {
         if (method == null)
            throw new ArgumentNullException(nameof(method));

         if (methods.Contains(method))
            return false;

         methods.Add(method);
         return true;
      }

      private void AddMethodFromField(FField field)
      {
         if (!field.GenerateFluentSetup)
            return;

         if (string.IsNullOrWhiteSpace(field.SetupMethodName))
            return;

         if (string.IsNullOrWhiteSpace(field.TypeName))
            return;

         var method = new FFluentSetupMethod(this, field.SetupMethodName, field.Type, ClassSymbol)
         {
            Source = field, Category = field.SetupMethodName
         };

         if (AddMethod(method))
         {
            if (field.IsListMember)
            {
               var withElement = new FWithListElementMethod(this, field) { Category = method.Category };
               AddMethod(withElement);
            }

            method.SetupIndicatorField = new FField(Context.BooleanType, $"{field.Name}WasSet");
            AddField(method.SetupIndicatorField);

            var fGetOrThrow = new FGetValueMethod(this, field) { Category = method.Category };
            AddMethod(fGetOrThrow);

            if (!field.HasDefaultValue)
            {
               var getMember = new FGetValueOrDefaultMethod(this, field, method.SetupIndicatorField) { Category = method.Category };
               AddMethod(getMember);
            }

            if (CanAddSet(field))
            {
               var setMember = new FSetupMemberMethod(this, field, method.SetupIndicatorField) { Category = method.Category };
               AddMethod(setMember);
            }
         }
      }

      private void AddSetupMethodFromProperty(FProperty property)
      {
         if (!property.RequiredSetupGeneration())
            return;

         var method = new FFluentSetupMethod(this, property.GetSetupMethodName(), property.Type, ClassSymbol) { Source = property };
         if (AddMethod(method))
         {
            method.SetupIndicatorField = new FField(Context.BooleanType, $"{property.Name.ToFirstLower()}WasSet");
            AddField(method.SetupIndicatorField);

            var fGetOrThrow = new FGetValueMethod(this, property) { Category = method.Category };
            AddMethod(fGetOrThrow);

            if (!property.HasDefaultValue)
            {
               var getMember = new FGetValueOrDefaultMethod(this, property, method.SetupIndicatorField) { Category = method.Category };
               AddMethod(getMember);
            }

            if (CanAddSet(property))
            {
               var setMember = new FSetupMemberMethod(this, property, method.SetupIndicatorField) { Category = method.Category };
               AddMethod(setMember);
            }
         }
      }

      private void AppendRequiredUsingDirectives(StringBuilder sourceBuilder)
      {
         sourceBuilder.AppendLine("using System;");
         sourceBuilder.AppendLine("using System.Runtime.CompilerServices;");
         sourceBuilder.AppendLine("using System.Collections.Generic;");
         sourceBuilder.AppendLine("using FluentSetups;");
         if (TargetAvailable() && !string.IsNullOrWhiteSpace(TargetTypeNamespace))
            sourceBuilder.AppendLine($"using {TargetTypeNamespace};");
      }

      private bool CanAddSet(IFluentMember field)
      {
         if (!TargetAvailable())
            return false;

         return Target.HasAccessibleProperty(field.Name.ToFirstUpper());
      }

      private void CloseNamespace(StringBuilder sourceBuilder)
      {
         if (!string.IsNullOrWhiteSpace(ContainingNamespace))
            sourceBuilder.AppendLine("}");
      }

      private string ComputeEntryMethodName()
      {
         if (ClassName.EndsWith("Setup"))
            return ClassName.Substring(0, ClassName.Length - 5);

         return ClassName;
      }

      private string ComputeNamespace()
      {
         var namespaceSymbol = ClassSymbol.ContainingNamespace;
         return namespaceSymbol.IsGlobalNamespace ? null : namespaceSymbol.ToString();
      }

      private string ComputeSetupMethod()
      {
         return FluentSetupAttribute.GetSetupMethod() ?? TargetTypeName ?? ComputeEntryMethodName();
      }

      private bool ContainsUserDefinedTargetBuilder()
      {
         return Methods.Any(m => m.Name == "Done" && m.ParameterCount == 0);
      }

      private string CreateConstructorCall()
      {
         var arguments = string.Join(", ", Target.ConstructorParameters.Select(p => $"Get{p.Name.ToFirstUpper()}(null)"));
         var builder = new StringBuilder($"new {Target.TypeName}({arguments})");
         return builder.ToString();
      }

      private FExistingMethod CreateMethod(IMethodSymbol methodSymbol)
      {
         return new FExistingMethod(this, methodSymbol);
      }

      private void FillMembers()
      {
         InitializeTarget();
         InitializeWithExistingMembers();
         UpdateMembersToGenerate();
         UpdateTargetBuilders();
      }

      private AttributeData FindMemberAttribute(ISymbol symbol)
      {
         return symbol.GetAttributes().Where(x => x.AttributeClass != null).FirstOrDefault(attribute =>
            Context.FluentMemberAttribute.Equals(attribute.AttributeClass, SymbolEqualityComparer.Default));
      }

      private void GenerateFields(StringBuilder sourceBuilder)
      {
         var fieldsToGenerate = Fields.Where(x => !x.IsUserDefined).ToArray();
         if (fieldsToGenerate.Length == 0)
            return;

         sourceBuilder.AppendLine("#region Fields");

         foreach (var field in fieldsToGenerate)
            sourceBuilder.AppendLine(field.ToCode());

         sourceBuilder.AppendLine("#endregion");
      }

      private void GenerateSetupMembers(StringBuilder sourceBuilder)
      {
         GenerateFields(sourceBuilder);
         GenerateSetupMethods(sourceBuilder);
         GenerateTargetCreation(sourceBuilder);
      }

      private void GenerateSetupMethods(StringBuilder sourceBuilder)
      {
         var methodGroups = Methods.Where(x => !x.IsUserDefined).GroupBy(x => x.Category).ToArray();

         foreach (var methodGroup in methodGroups)
         {
            sourceBuilder.AppendLine($"#region {methodGroup.Key}");

            foreach (var method in methodGroup)
               sourceBuilder.AppendLine(method.ToCode());

            sourceBuilder.AppendLine("#endregion");
         }
      }

      private void GenerateTargetCreation(StringBuilder sourceBuilder)
      {
         if (!TargetCreationPossible(this))
            return;

         if (ContainsUserDefinedTargetBuilder())
            return;

         sourceBuilder.AppendLine($"public {TargetTypeName} Done()");
         sourceBuilder.AppendLine("{");
         sourceBuilder.AppendLine($"   var target = {CreateConstructorCall()};");
         sourceBuilder.AppendLine("   return target;");
         sourceBuilder.AppendLine("}");
      }

      private void InitializeTarget()
      {
         var targetType = FluentSetupAttribute.GetTargetType();
         if (targetType.IsNull)
            return;

         var targetMode = FluentSetupAttribute.GetTargetMode();
         if (targetMode.Value is int enumValue)
            TargetMode = (TargetGenerationMode)enumValue;

         if (targetType.Value is INamedTypeSymbol typeSymbol)
         {
            TargetTypeNamespace = typeSymbol.ContainingNamespace.IsGlobalNamespace ? null : typeSymbol.ContainingNamespace.ToString();
            Target = new FTarget(this, typeSymbol);
         }
      }

      private void InitializeWithExistingMembers()
      {
         foreach (var member in ClassSymbol.GetMembers())
            AddMember(member);
      }

      private void OpenClass(StringBuilder sourceBuilder)
      {
         sourceBuilder.AppendLine("/// <summary>Automatic generated class part by fluent setups</summary>");
         sourceBuilder.AppendLine("[CompilerGenerated]");
         sourceBuilder.AppendLine($"{Modifier} partial class {ClassName}");
         sourceBuilder.AppendLine("{");
      }

      private void OpenNamespace(StringBuilder sourceBuilder)
      {
         sourceBuilder.AppendLine("/// To get help see https://github.com/bramerdaniel/FluentSetups");
         if (!string.IsNullOrWhiteSpace(ContainingNamespace))
         {
            sourceBuilder.AppendLine($"namespace {ContainingNamespace}");
            sourceBuilder.AppendLine("{");
         }
      }

      private bool TargetAvailable()
      {
         if (TargetMode == TargetGenerationMode.Disabled)
            return false;

         return !string.IsNullOrWhiteSpace(TargetTypeName);
      }

      private void UpdateFromConstructor()
      {
         if (Target?.Constructor == null)
            return;

         foreach (var constructorParameter in Target.Constructor.Parameters)
         {
            var backingField = FField.ForConstructorParameter(constructorParameter, Context);
            if (AddField(backingField))
               AddMethodFromField(backingField);
         }
      }

      private void UpdateFromTarget()
      {
         if (Target == null)
            return;

         UpdateFromConstructor();

         foreach (var property in Target.Properties)
         {
            var existingProperty = Properties.FirstOrDefault(p => p.Name == property.Name && p.TypeName == property.Type.ToString());
            if (existingProperty != null)
            {
               AddSetupMethodFromProperty(existingProperty);
            }
            else
            {
               var backingField = FField.ForTargetProperty(property, Context);
               if (AddField(backingField))
                  AddMethodFromField(backingField);
            }
         }
      }

      private void UpdateMembersToGenerate()
      {
         foreach (var field in Fields.ToArray())
            AddMethodFromField(field);

         foreach (var property in Properties)
            AddSetupMethodFromProperty(property);

         UpdateFromTarget();
      }

      private void UpdateTargetBuilders()
      {
         if (!TargetCreationPossible(this))
            return;

         AddMethod(new FDoneMethod(this));
         AddMethod(new FCreateTargetMethod(this));
         AddMethod(new FSetupTargetMethod(this));
      }

      #endregion
   }
}