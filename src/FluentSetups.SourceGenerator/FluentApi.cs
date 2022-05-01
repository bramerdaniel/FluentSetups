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

      public static FluentApi FromExecutionContext(GeneratorExecutionContext context)
      {
         return new FluentApi
         {
            Context = context,
            FluentSetupAttribute = context.Compilation.GetTypeByMetadataName(FluentSetupAttributeName),
            FluentPropertyAttribute = context.Compilation.GetTypeByMetadataName(FluentPropertyAttributeName)
         };
      }

      public GeneratorExecutionContext Context { get; set; }

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
         setupClassInfo = null;
         var semanticModel = Context.Compilation.GetSemanticModel(candidate.SyntaxTree);

         var classSymbol = (ITypeSymbol)semanticModel.GetDeclaredSymbol(candidate);
         if (classSymbol == null)
            return false;

         var fluentSetupAttribute = classSymbol.GetAttributes().FirstOrDefault(IsFluentSetupAttribute);
         if (fluentSetupAttribute == null)
            return false;

         setupClassInfo = new SetupClassInfo
         {
            ClassSyntax = candidate,
            ClassModel = semanticModel,
            FluentSetupAttribute = fluentSetupAttribute
         };

         return true;

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
         var semanticModel = Context.Compilation.GetSemanticModel(candidate.SyntaxTree);

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