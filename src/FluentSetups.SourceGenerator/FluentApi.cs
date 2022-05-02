// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentApi.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using System.Collections.Generic;
   using System.Linq;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp.Syntax;

   internal struct FluentApi
   {
      public static string FluentSetupAttributeName => "FluentSetups.FluentSetupAttribute";

      public static string FluentPropertyAttributeName => "FluentSetups.FluentPropertyAttribute";

      public INamedTypeSymbol FluentSetupAttribute { get; set; }

      public INamedTypeSymbol FluentPropertyAttribute { get; set; }

      public static FluentApi FromCompilation(Compilation compilation)
      {
         return new FluentApi
         {
            Compilation = compilation,
            FluentSetupAttribute = compilation.GetTypeByMetadataName(FluentSetupAttributeName),
            FluentPropertyAttribute = compilation.GetTypeByMetadataName(FluentPropertyAttributeName)
         };
      }

      public Compilation Compilation { get; set; }


      public bool TryGetMissingType(out string missingType)
      {
         if (FluentSetupAttribute == null)
         {
            missingType = FluentSetupAttributeName;
            return true;
         }

         if (FluentPropertyAttribute == null)
         {
            missingType = FluentPropertyAttributeName;
            return true;
         }

         missingType = null;
         return false;
      }

      public IEnumerable<SetupClassInfo> FindFluentSetups(IEnumerable<ClassDeclarationSyntax> setupCandidates)
      {
         foreach (var setupCandidate in setupCandidates)
         {
            if (TryGetSetupClass(setupCandidate, out SetupClassInfo classInfo))
               yield return classInfo;
         }
      }

      private bool TryGetSetupClass(ClassDeclarationSyntax candidate, out SetupClassInfo setupClassInfo)
      {
         var semanticModel = Compilation.GetSemanticModel(candidate.SyntaxTree);
         setupClassInfo = new SetupClassInfo(this, candidate, semanticModel);

         return setupClassInfo.IsValidSetup();
      }

      private bool IsFluentSetupAttribute(AttributeData attributeData)
      {
         if (FluentSetupAttribute.Equals(attributeData.AttributeClass, SymbolEqualityComparer.Default))
            return true;

         var attributeName = attributeData.AttributeClass?.Name;
         if (attributeName == "FluentSetupAttribute")
            return true;

         if (attributeName == "FluentSetup")
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
   }
}