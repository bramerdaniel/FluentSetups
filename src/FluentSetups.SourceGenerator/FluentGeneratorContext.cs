// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentGeneratorContext.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using System.Collections.Generic;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp.Syntax;

   internal struct FluentGeneratorContext
   {
      #region Public Properties

      public Compilation Compilation { get; set; }
      
      #endregion

      #region Properties

      internal static string FluentEntryNamespaceAttributeName => "FluentSetups.FluentEntryNamespaceAttribute";

      internal static string FluentMemberAttributeName => "FluentSetups.FluentMemberAttribute";

      internal static string FluentSetupAttributeName => "FluentSetups.FluentSetupAttribute";

      internal INamedTypeSymbol FluentEntryNamespaceAttribute { get; set; }

      internal INamedTypeSymbol FluentPropertyAttribute { get; set; }

      internal INamedTypeSymbol FluentSetupAttribute { get; set; }

      #endregion

      #region Public Methods and Operators

      public static FluentGeneratorContext FromCompilation(Compilation compilation)
      {
         return new FluentGeneratorContext
         {
            Compilation = compilation,
            FluentEntryNamespaceAttribute = compilation.GetTypeByMetadataName(FluentEntryNamespaceAttributeName),
            FluentSetupAttribute = compilation.GetTypeByMetadataName(FluentSetupAttributeName),
            FluentPropertyAttribute = compilation.GetTypeByMetadataName(FluentMemberAttributeName)
         };
      }
      
      public IEnumerable<SetupClassInfo> FindFluentSetups(IEnumerable<ClassDeclarationSyntax> setupCandidates)
      {
         foreach (var setupCandidate in setupCandidates)
         {
            if (TryGetSetupClass(setupCandidate, out SetupClassInfo classInfo))
               yield return classInfo;
         }
      }

      public bool TryGetMissingType(out string missingType)
      {
         if (FluentSetupAttribute == null)
         {
            missingType = FluentSetupAttributeName;
            return true;
         }

         if (FluentPropertyAttribute == null)
         {
            missingType = FluentMemberAttributeName;
            return true;
         }

         missingType = null;
         return false;
      }

      #endregion

      #region Methods

      private bool IsFluentSetupAttribute(AttributeData attributeData)
      {
         if (FluentSetupAttribute.Equals(attributeData.AttributeClass, SymbolEqualityComparer.Default))
            return true;
         return false;
      }

      private bool IsSetupClass(ClassDeclarationSyntax candidate)
      {
         var semanticModel = Compilation.GetSemanticModel(candidate.SyntaxTree);

         var classSymbol = (ITypeSymbol)semanticModel.GetDeclaredSymbol(candidate);
         if (classSymbol == null)
            return false;

         foreach (var attributeData in classSymbol.GetAttributes())
         {
            if (FluentSetupAttribute.Equals(attributeData.AttributeClass, SymbolEqualityComparer.Default))
               return true;

            var attributeName = attributeData.AttributeClass?.Name;
            if (attributeName == "FluentSetupAttribute")
               return true;

            if (attributeName == "FluentSetup")
               return true;
         }

         return false;
      }

      private bool TryGetSetupClass(ClassDeclarationSyntax candidate, out SetupClassInfo setupClassInfo)
      {
         var semanticModel = Compilation.GetSemanticModel(candidate.SyntaxTree);
         setupClassInfo = new SetupClassInfo(this, candidate, semanticModel);

         return setupClassInfo.IsValidSetup();
      }

      #endregion
      
   }
}