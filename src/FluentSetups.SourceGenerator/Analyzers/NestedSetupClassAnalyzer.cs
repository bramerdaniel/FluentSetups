﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NestedSetupClassAnalyzer.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Analyzers
{
   using System.Collections.Immutable;
   using System.Linq;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.Diagnostics;

   [DiagnosticAnalyzer(LanguageNames.CSharp)]
   public class NestedSetupClassAnalyzer : DiagnosticAnalyzer
   {
      #region Public Properties

      /// <summary>Returns a set of descriptors for the diagnostics that this analyzer is capable of producing.</summary>
      public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
         ImmutableArray.Create(FluentSetupDiagnostics.NotSupportedNestedSetup);

      #endregion

      #region Public Methods and Operators

      public override void Initialize(AnalysisContext context)
      {
         context.EnableConcurrentExecution();
         context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
         context.RegisterCompilationStartAction(compilationContext =>
         {
            // We only care about compilations where attribute type "FluentSetup" is available.
            var fluentSetupAttribute = compilationContext.Compilation.GetTypeByMetadataName(FluentGeneratorContext.FluentSetupAttributeName);
            if (fluentSetupAttribute == null)
               return;

            // Register an action that accesses the immutable state and reports diagnostics.
            compilationContext.RegisterSymbolAction(symbolContext => AnalyzeSymbol(symbolContext, fluentSetupAttribute), SymbolKind.NamedType);
         });
      }

      #endregion

      #region Methods

      private static Location FindLocation(AttributeData attributeData, INamedTypeSymbol ownerClass)
      {
         if (attributeData.ApplicationSyntaxReference != null)
            return Location.Create(attributeData.ApplicationSyntaxReference.SyntaxTree, attributeData.ApplicationSyntaxReference.Span);

         return ownerClass.Locations.FirstOrDefault() ?? Location.None;
      }

      private void AnalyzeSymbol(SymbolAnalysisContext context, INamedTypeSymbol fluentSetupAttribute)
      {
         var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
         if (namedTypeSymbol.ContainingType == null)
         {
            // we only care about nested classes here
            return;
         }

         var attributeData = namedTypeSymbol.GetAttributes().FirstOrDefault(IsFluentSetupAttribute);
         if (attributeData?.AttributeClass == null)
            return;

         var location = FindLocation(attributeData, namedTypeSymbol);

         var diagnostic = Diagnostic.Create(FluentSetupDiagnostics.NotSupportedNestedSetup, location);
         context.ReportDiagnostic(diagnostic);

         bool IsFluentSetupAttribute(AttributeData candidate)
         {
            return fluentSetupAttribute.Equals(candidate.AttributeClass, SymbolEqualityComparer.Default);
         }
      }

      #endregion
   }
}