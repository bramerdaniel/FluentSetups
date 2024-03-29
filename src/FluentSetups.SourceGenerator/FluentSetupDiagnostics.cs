﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentSetupDiagnostics.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using System.Diagnostics.CodeAnalysis;

   using Microsoft.CodeAnalysis;

   [SuppressMessage("MicrosoftCodeAnalysisReleaseTracking", "RS2008:Enable analyzer release tracking")]
   public static class FluentSetupDiagnostics
   {
      #region Constants and Fields

      internal static readonly DiagnosticDescriptor MultiplePartialParts = new DiagnosticDescriptor(id: "FSI0001",
         title: "FluentSetups source generator",
         messageFormat: "Fluent setup generation for class '{0}' is skipped du tue multiple partial members",
         category: "FluentSetups",
         defaultSeverity: DiagnosticSeverity.Info,
         isEnabledByDefault: true);

      internal static readonly DiagnosticDescriptor NotSupportedNestedSetup = new DiagnosticDescriptor(id: "FSW0001",
         title: "FluentSetups source generator",
         messageFormat: "Fluent setup generation for nested classes is not supported",
         category: "FluentSetups",
         defaultSeverity: DiagnosticSeverity.Warning,
         isEnabledByDefault: true);

      #endregion
   }
}