// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupClassInfo.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using System;
   using System.Linq;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp.Syntax;

   /// <summary>Data class containing all the required information along the generation process</summary>
   internal class SetupClassInfo
   {
      #region Constants and Fields

      private readonly FluentGeneratorContext fluentApi;

      #endregion

      #region Constructors and Destructors

      public SetupClassInfo(FluentGeneratorContext fluentApi, ClassDeclarationSyntax candidate, SemanticModel semanticModel)
      {
         this.fluentApi = fluentApi;

         ClassSyntax = candidate ?? throw new ArgumentNullException(nameof(candidate));
         SemanticModel = semanticModel ?? throw new ArgumentNullException(nameof(semanticModel));
         ClassSymbol = (ITypeSymbol)semanticModel.GetDeclaredSymbol(candidate);
         FluentSetupAttribute = ClassSymbol?.GetAttributes().FirstOrDefault(IsFluentSetupAttribute);
      }

      #endregion

      #region Public Properties

      public string ClassName => ClassSyntax.Identifier.Text;

      /// <summary>Gets or sets the class symbol.</summary>
      public ITypeSymbol ClassSymbol { get; }

      public ClassDeclarationSyntax ClassSyntax { get; }

      public AttributeData FluentSetupAttribute { get; }

      public SemanticModel SemanticModel { get; }

      #endregion

      #region Public Methods and Operators

      public bool IsValidSetup()
      {
         if (ClassSymbol == null)
            return false;

         if (FluentSetupAttribute == null)
            return false;

         return true;
      }

      #endregion

      #region Methods

      internal static void InitializeFromCompilation(Compilation compilation)
      {
      }

      internal string GetSetupEntryClassName()
      {
         if (FluentSetupAttribute == null)
            return null;

         var firstArgument = FluentSetupAttribute.ConstructorArguments.FirstOrDefault();
         if (firstArgument.IsNull)
            return "Setup";

         return firstArgument.Value?.ToString() ?? "Setup";
      }

      internal string GetSetupEntryNameSpace()
      {
         if (FluentSetupAttribute == null)
            return null;

         var firstArgument = FluentSetupAttribute.NamedArguments.FirstOrDefault(x => x.Key == "EntryNamespace");
         if (firstArgument.Value.Value is string value)
            return value;

         // This should be the default namespace of the containing assembly
         return ClassSymbol.ContainingAssembly.MetadataName;
      }

      private bool IsFluentSetupAttribute(AttributeData attributeData)
      {
         return IsFluentSetupAttribute(attributeData.AttributeClass);
      }

      private bool IsFluentSetupAttribute(INamedTypeSymbol attributeSymbol)
      {
         return fluentApi.FluentSetupAttribute.Equals(attributeSymbol, SymbolEqualityComparer.Default);
      }

      #endregion
   }
}