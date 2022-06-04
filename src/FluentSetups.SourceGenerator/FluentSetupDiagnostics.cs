﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentSetupDiagnostics.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using Microsoft.CodeAnalysis;

   public class FluentSetupDiagnostics
   {
      internal static readonly DiagnosticDescriptor MultiplePartialParts = new DiagnosticDescriptor(id: "FSI0001",
         title: "FluentSetups source generator",
         messageFormat: "Fluent setup generation for class {0} is skipped du tue multiple partial members",
         category: "FluentSetups",
         defaultSeverity: DiagnosticSeverity.Info,
         isEnabledByDefault: true);
   }
}