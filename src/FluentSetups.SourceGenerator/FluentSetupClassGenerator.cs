// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentSetupClassGenerator.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using System;
   using System.Linq;
   using System.Text;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp;
   using Microsoft.CodeAnalysis.CSharp.Syntax;
   using Microsoft.CodeAnalysis.Text;

   internal class FluentSetupClassGenerator
   {
      private readonly FluentApi fluentApi;

      #region Constants and Fields

      #endregion

      #region Constructors and Destructors

      public FluentSetupClassGenerator(GeneratorExecutionContext context, ClassDeclarationSyntax classDeclarationSyntax, FluentApi fluentApi)
      {
         this.fluentApi = fluentApi;
         GeneratorContext = context;
         ClassDeclarationSyntax = classDeclarationSyntax;
         var semanticModel = GeneratorContext.Compilation.GetSemanticModel(ClassDeclarationSyntax.SyntaxTree);
         ClassSymbol = semanticModel.GetDeclaredSymbol(ClassDeclarationSyntax, GeneratorContext.CancellationToken);
         SetupTypeName = FindSetupType();
      }

      private ITypeSymbol FindSetupType()
      {
         var typeByMetadataName = GeneratorContext.Compilation.GetTypeByMetadataName("FluentSetups.IFluentSetup<T>");

         foreach (var namedTypeSymbol in ClassSymbol.Interfaces)
         {
            var name = namedTypeSymbol.Name;
            if (name == "IFluentSetup" && namedTypeSymbol.TypeArguments.Length == 1)
            {
               var targetType = namedTypeSymbol.TypeArguments[0];
               
               return targetType;
            }
            
         }

         return ClassSymbol.BaseType.TypeArguments.FirstOrDefault();
      }

      public ITypeSymbol SetupTypeName { get; set; }

      #endregion

      #region Public Properties

      public ClassDeclarationSyntax ClassDeclarationSyntax { get; }

      public ITypeSymbol ClassSymbol { get; }

      public GeneratorExecutionContext GeneratorContext { get; }

      #endregion

      #region Public Methods and Operators

      public void Execute()
      {
         if (ClassSymbol == null)
            return;

         string source = $@"
         namespace {ClassSymbol.ContainingNamespace}
         {{ 
            using System;

            /// <summary>Automatic generated class part by fluent setups</summary>
            public partial class {ClassSymbol.Name}
            {{
                {GenerateFluentMethods(ClassSymbol)}
                {GenerateTargetFactoryMethods(SetupTypeName)}
            }}
         }}";

         var syntaxTree = CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace();
         source = syntaxTree.ToString();

         GeneratorContext.AddSource($"{ClassSymbol.Name}.generated.cs", SourceText.From(source, Encoding.UTF8));
      }

      private string GenerateTargetFactoryMethods(ITypeSymbol setupTypeName)
      {
         if (setupTypeName == null)
            return string.Empty;

         return  $@"public {SetupTypeName} Done()
                 {{
                     var instance = CreateInstance();
                     return instance;
                 }}

                 internal partial {SetupTypeName} CreateInstance();";
      }

      #endregion

      #region Methods

      private string CreateFluentPropertySetup(ITypeSymbol classSymbol, IPropertySymbol propertySymbol, AttributeData fluentPropertyAttribute)
      {
         if (propertySymbol.SetMethod == null)
            return string.Empty;

         var propertyBuilder = new StringBuilder();
         string setupMethodName = ComputeSetupName(propertySymbol, fluentPropertyAttribute);
         propertyBuilder.AppendLine($"public {classSymbol.Name} {setupMethodName}({propertySymbol.Type} value)");
         propertyBuilder.AppendLine("{");

         var validation = CreateParameterValidation(propertySymbol, "value");
         if (!string.IsNullOrWhiteSpace(validation))
            propertyBuilder.AppendLine(validation);

         propertyBuilder.AppendLine($"{propertySymbol.Name} = value;");
         propertyBuilder.AppendLine("return this;");
         propertyBuilder.AppendLine("}");

         return propertyBuilder.ToString();
      }

      private string ComputeSetupName(IPropertySymbol property, AttributeData attributeData)
      {
         if (attributeData.ConstructorArguments.Length > 0)
            return attributeData.ConstructorArguments[0].Value.ToString();

         return $"With{property.Name}";
      }

      private string CreateParameterValidation(IPropertySymbol propertySymbol, string parameterName)
      {
         if (propertySymbol == null)
            throw new ArgumentNullException(nameof(propertySymbol));

         if (propertySymbol.Type.IsValueType)
            return null;

         var validation = new StringBuilder();
         validation.AppendLine($"if ({parameterName} == null)");
         validation.AppendLine($"throw new ArgumentNullException(nameof({parameterName}));");
         return validation.ToString();
      }

      private string GenerateFluentMethods(ITypeSymbol classSymbol)
      {
         var propertyBuilder = new StringBuilder();
         foreach (var propertySymbol in classSymbol.GetMembers().OfType<IPropertySymbol>())
         {
            if (IsFluentProperty(propertySymbol, out var fluentPropertyAttribute))
            {
               var propertySetupMethod = CreateFluentPropertySetup(classSymbol, propertySymbol , fluentPropertyAttribute);
               if (!string.IsNullOrWhiteSpace(propertySetupMethod))
                  propertyBuilder.AppendLine(propertySetupMethod);
            }
         }

         return propertyBuilder.ToString();
      }

      private bool IsFluentProperty(IPropertySymbol propertySymbol, out AttributeData attributeData)
      {
         foreach (var attribute in propertySymbol.GetAttributes())
         {
            if (attribute.AttributeClass == null)
               continue;

            if (fluentApi.FluentPropertyAttribute.Equals(attribute.AttributeClass, SymbolEqualityComparer.Default))
            {
               attributeData = attribute;
               return true;
            }
         }

         attributeData = null;
         return false;
      }

      #endregion
   }
}