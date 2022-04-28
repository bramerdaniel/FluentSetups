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
      #region Constants and Fields

      #endregion

      #region Constructors and Destructors

      public FluentSetupClassGenerator(GeneratorExecutionContext context, ClassDeclarationSyntax classDeclarationSyntax)
      {
         GeneratorContext = context;
         ClassDeclarationSyntax = classDeclarationSyntax;
         var semanticModel = GeneratorContext.Compilation.GetSemanticModel(ClassDeclarationSyntax.SyntaxTree);
         ClassSymbol = semanticModel.GetDeclaredSymbol(ClassDeclarationSyntax, GeneratorContext.CancellationToken);
         SetupTypeName = ClassSymbol.BaseType.TypeArguments.FirstOrDefault();
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
            /// <summary>Automatic generated class part by fluent setups</summary>
            public partial class {ClassSymbol.Name}
             {{
                 {GenerateFluentMethods(ClassSymbol)}
                 public {SetupTypeName} Done()
                 {{
                     // This is so cool
                     var instance = CreateInstance();
                     return instance;
                 }}
             }}

             internal partial {SetupTypeName} CreateInstance();
         }}";

         var syntaxTree = CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace();
         source = syntaxTree.ToString();

         GeneratorContext.AddSource($"{ClassSymbol.Name}.generated.cs", SourceText.From(source, Encoding.UTF8));
      }

      #endregion

      #region Methods

      private string CreateFluentPropertySetup(ITypeSymbol classSymbol, IPropertySymbol propertySymbol)
      {
         if (propertySymbol.SetMethod == null)
         {
            return string.Empty;
         }

         var propertyBuilder = new StringBuilder();

         propertyBuilder.AppendLine($"public {classSymbol.Name} With{propertySymbol.Name}({propertySymbol.Type} value)");
         propertyBuilder.AppendLine("{");

         var validation = CreateParameterValidation(propertySymbol, "value");
         if (!string.IsNullOrWhiteSpace(validation))
            propertyBuilder.AppendLine(validation);

         propertyBuilder.AppendLine($"{propertySymbol.Name} = value;");
         propertyBuilder.AppendLine("return this;");
         propertyBuilder.AppendLine("}");

         return propertyBuilder.ToString();
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
            if (IsFluentProperty(propertySymbol))
            {
               var propertySetupMethod = CreateFluentPropertySetup(classSymbol, propertySymbol);
               if (!string.IsNullOrWhiteSpace(propertySetupMethod))
                  propertyBuilder.AppendLine(propertySetupMethod);
            }
         }

         return propertyBuilder.ToString();
      }

      private bool IsFluentProperty(IPropertySymbol propertySymbol)
      {
         foreach (var attribute in propertySymbol.GetAttributes())
         {
            if (attribute.AttributeClass != null && attribute.AttributeClass.Name == "FluentProperty")
               return true;
         }

         return false;
      }

      #endregion
   }
}