// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentGeneratorContext.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using System;
   using System.Collections.Generic;
   using System.Linq;

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

      internal INamedTypeSymbol FluentMemberAttribute { get; set; }

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
            FluentMemberAttribute = compilation.GetTypeByMetadataName(FluentMemberAttributeName)
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
      
      public SetupClassInfo CreateFluentSetupInfo(ClassDeclarationSyntax setupCandidate)
      {
         if (TryGetSetupClass(setupCandidate, out SetupClassInfo classInfo))
            return classInfo;

         throw new ArgumentException($"The specified {nameof(ClassDeclarationSyntax)} is not a fluent setup class", nameof(setupCandidate));
      }

      public bool TryGetMissingType(out string missingType)
      {
         if (FluentSetupAttribute == null)
         {
            missingType = FluentSetupAttributeName;
            return true;
         }

         if (FluentMemberAttribute == null)
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
         setupClassInfo = null;
         var semanticModel = Compilation.GetSemanticModel(candidate.SyntaxTree);

         if (!(semanticModel.GetDeclaredSymbol(candidate) is ITypeSymbol classSymbol))
            return false;

         var fluentAttribute = classSymbol.GetAttributes().FirstOrDefault(IsFluentSetupAttribute);
         if (fluentAttribute == null)
            return false;

         setupClassInfo = new SetupClassInfo(this, candidate, semanticModel, classSymbol, fluentAttribute);
         return true;
      }

      #endregion
      
   }
}