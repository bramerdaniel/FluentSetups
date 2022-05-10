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
   [System.Diagnostics.CodeAnalysis.SuppressMessage("sonar", "S3267:Loops should be simplified with \"LINQ\" expressions")]
   internal class FClass
   {
      #region Constants and Fields

      private readonly ITypeSymbol classSymbol;

      private readonly List<FField> fields = new List<FField>(5);

      private readonly AttributeData fluentSetupAttribute;

      private readonly List<FMethod> methods = new List<FMethod>(5);

      private readonly List<FProperty> properties = new List<FProperty>(5);

      #endregion

      #region Constructors and Destructors

      private FClass(FluentGeneratorContext context, ITypeSymbol classSymbol, AttributeData fluentSetupAttribute)
      {
         Context = context;
         this.classSymbol = classSymbol ?? throw new ArgumentNullException(nameof(classSymbol));
         this.fluentSetupAttribute = fluentSetupAttribute ?? throw new ArgumentNullException(nameof(fluentSetupAttribute));

         ClassName = classSymbol.Name;
         ContainingNamespace = ComputeNamespace();
         Modifier = ComputeModifier(classSymbol);
         FillMembers();
      }

      #endregion

      #region Public Properties

      public string ClassName { get; }

      public string ContainingNamespace { get; }

      public FluentGeneratorContext Context { get; }

      public string EntryClassName { get; private set; }

      public string EntryClassNamespace { get; private set; }

      public IReadOnlyList<FField> Fields => fields;

      public IReadOnlyList<FMethod> Methods => methods;

      public string Modifier { get; set; } = "internal";

      public IReadOnlyList<FProperty> Properties => properties;

      public FTarget Target { get; private set; }

      public TargetGenerationMode TargetMode { get; set; }

      public string TargetTypeName => Target?.TypeName;

      public string TargetTypeNamespace { get; set; }

      #endregion

      #region Public Methods and Operators

      public static FClass Create(FluentGeneratorContext context, SetupClassInfo classInfo)
      {
         var classModel = new FClass(context, classInfo.ClassSymbol, classInfo.FluentSetupAttribute)
         {
            EntryClassNamespace = classInfo.GetSetupEntryNameSpace(),
            EntryClassName = classInfo.GetSetupEntryClassName(),
         };

         return classModel;
      }

      public string ToCode()
      {
         var sourceBuilder = new StringBuilder();
         OpenNamespace(sourceBuilder);
         {
            AppendRequiredUsingDirectives(sourceBuilder);
            OpenClass(sourceBuilder);

            GenerateSetupMembers(sourceBuilder);

            CloseClass(sourceBuilder);
         }
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
                  var method = new FMethod(methodSymbol);
                  methods.Add(method);
                  break;
               }
         }
      }

      private void AddMethodFromField(FField field)
      {
         if (string.IsNullOrWhiteSpace(field.SetupMethodName))
            return;

         if (string.IsNullOrWhiteSpace(field.TypeName))
            return;

         var method = new FMethod(field.SetupMethodName, field.Type, classSymbol);
         methods.Add(method);
      }

      private void AppendRequiredUsingDirectives(StringBuilder sourceBuilder)
      {
         sourceBuilder.AppendLine("using System;");
         sourceBuilder.AppendLine("using System.Runtime.CompilerServices;");
         if (TargetCreationPossible() && !string.IsNullOrWhiteSpace(TargetTypeNamespace))
            sourceBuilder.AppendLine($"using {TargetTypeNamespace};");
      }

      private void CloseNamespace(StringBuilder sourceBuilder)
      {
         if (!string.IsNullOrWhiteSpace(ContainingNamespace))
            sourceBuilder.AppendLine("}");
      }

      private IEnumerable<FField> ComputeFieldSetups(SetupClassInfo classInfo)
      {
         foreach (var fieldSymbol in classInfo.ClassSymbol.GetMembers().OfType<IFieldSymbol>())
            yield return new FField(fieldSymbol, FindMemberAttribute(fieldSymbol));
      }

      private string ComputeNamespace()
      {
         var namespaceSymbol = classSymbol.ContainingNamespace;
         return namespaceSymbol.IsGlobalNamespace ? null : namespaceSymbol.ToString();
      }

      private bool ContainsUserDefinedTargetBuilder()
      {
         return Methods.Any(m => m.MemberName == "Done" && m.TypeName == null);
      }

      private void FillMembers()
      {
         FillTargetTypeProperties();
         foreach (var member in classSymbol.GetMembers())
            AddMember(member);

         UpdateMembersToGenerate();
      }

      private void FillTargetTypeProperties()
      {
         var targetType = fluentSetupAttribute.GetTargetType();
         if (targetType.IsNull)
            return;

         var targetMode = fluentSetupAttribute.GetTargetMode();
         if (targetMode.Value is int enumValue)
            TargetMode = (TargetGenerationMode)enumValue;

         if (targetType.Value is INamedTypeSymbol typeSymbol)
         {
            TargetTypeNamespace = typeSymbol.ContainingNamespace.IsGlobalNamespace ? null : typeSymbol.ContainingNamespace.ToString();
            Target = new FTarget(this, typeSymbol);
         }
      }

      private AttributeData FindMemberAttribute(ISymbol symbol)
      {
         return symbol.GetAttributes().Where(x => x.AttributeClass != null)
            .FirstOrDefault(attribute => Context.FluentMemberAttribute.Equals(attribute.AttributeClass, SymbolEqualityComparer.Default));
      }

      private void GenerateFields(StringBuilder sourceBuilder)
      {
         sourceBuilder.AppendLine("#region Fields");

         foreach (var field in Fields.Where(x => !x.IsUserDefined))
            sourceBuilder.AppendLine(field.ToCode());

         sourceBuilder.AppendLine("#endregion");
      }

      private void GenerateSetupMembers(StringBuilder sourceBuilder)
      {
         GenerateFields(sourceBuilder);
         GenerateSetupMethods(sourceBuilder);

         //foreach (var member in Fields)
         //{
         //   GenerateMemberSetup(sourceBuilder, member, false);
         //}

         //foreach (var member in Properties)
         //   GenerateMemberSetup(sourceBuilder, member, false);

         //if (Target != null)
         //   foreach (var member in Target.Properties)
         //      GenerateMemberSetup(sourceBuilder, member, true);

         GenerateTargetCreation(sourceBuilder);
      }

      private void GenerateSetupMethods(StringBuilder sourceBuilder)
      {
         var methodsToGenerate = Methods.Where(x => !x.IsUserDefined).ToArray();

         foreach (var method in methodsToGenerate)
            sourceBuilder.AppendLine(method.ToCode());
      }

      private void GenerateTargetCreation(StringBuilder sourceBuilder)
      {
         if (!TargetCreationPossible(this))
            return;

         if (ContainsUserDefinedTargetBuilder())
            return;

         sourceBuilder.AppendLine($"public {TargetTypeName} Done()");
         sourceBuilder.AppendLine("{");
         sourceBuilder.AppendLine($"   var target = new {TargetTypeName}();");
         sourceBuilder.AppendLine($"   return target;");
         sourceBuilder.AppendLine("}");
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
         if (!string.IsNullOrWhiteSpace(ContainingNamespace))
         {
            sourceBuilder.AppendLine($"namespace {ContainingNamespace}");
            sourceBuilder.AppendLine("{");
         }
      }

      private bool TargetCreationPossible()
      {
         if (TargetMode == TargetGenerationMode.Disabled)
            return false;

         return !string.IsNullOrWhiteSpace(TargetTypeName);
      }

      private void UpdateMembersToGenerate()
      {
         foreach (var field in Fields)
         {
            if (field.RequiredSetupGeneration())
               AddMethodFromField(field);
         }

         foreach (var property in Properties)
            AddSetupMethodFromProperty(property);

         UpdateFromTarget();
      }

      private void AddSetupMethodFromProperty(FProperty property)
      {
         if (!property.RequiredSetupGeneration())
            return;

         var setupMethod = new FMethod(property.GetSetupMethodName(), property.Type, classSymbol);
         AddMethod(setupMethod);
      }

      private void UpdateFromTarget()
      {
         if (Target == null)
            return;

         foreach (var property in Target.Properties)
         {
            var method = new FMethod(property.SetupMethodName, property.Type, classSymbol);
            AddMethod(method);
            AddField(FField.ForProperty(property));
         }
      }

      private void AddField(FField field)
      {
         if (field == null)
            throw new ArgumentNullException(nameof(field));

         if (fields.Contains(field))
            return;

         fields.Add(field);
      }

      private void AddMethod(FMethod method)
      {
         if (method == null)
            throw new ArgumentNullException(nameof(method));

         if (methods.Contains(method))
            return;

         methods.Add(method);
      }

      #endregion
   }
}