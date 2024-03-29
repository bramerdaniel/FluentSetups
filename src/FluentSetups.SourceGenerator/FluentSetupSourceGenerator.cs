﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentSetupSourceGenerator.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using System.Linq;
   using System.Text;

   using FluentSetups.SourceGenerator.Models;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.Text;

   [Generator]
   public class FluentSetupSourceGenerator : ISourceGenerator
   {
      #region ISourceGenerator Members

      public void Initialize(GeneratorInitializationContext context)
      {
         context.RegisterForSyntaxNotifications(() => new FluentSetupSyntaxReceiver());
      }

      public void Execute(GeneratorExecutionContext context)
      {
         if (!(context.SyntaxReceiver is FluentSetupSyntaxReceiver syntaxReceiver))
            return;

         var fluentContext = FluentGeneratorContext.FromCompilation(context.Compilation);
         if (fluentContext.TryGetMissingType(out string missingType))
         {
            MissingReferenceDiagnostic(context, missingType);
            return;
         }

         RunSourceGeneration(context, fluentContext, syntaxReceiver);
      }

      #endregion

      #region Methods

      private static void AddSourceOrReportError(GeneratorExecutionContext context, GeneratedSource generatedSource)
      {
         if (generatedSource.Enabled)
            context.AddSource(generatedSource.Name, SourceText.From(generatedSource.Code, Encoding.UTF8));

         foreach (var diagnostic in generatedSource.Diagnostics)
            context.ReportDiagnostic(diagnostic);
      }

      private void MissingReferenceDiagnostic(GeneratorExecutionContext context, string attributeName)
      {
         var missingReference = new DiagnosticDescriptor(id: "FS0001", title: "fluent source generator failed",
            messageFormat: "Could not find the '{0}' attribute. Are you missing a reference?",
            category: nameof(FluentSetupSourceGenerator),
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

         context.ReportDiagnostic(Diagnostic.Create(missingReference, Location.None, attributeName));
      }

      private void RunSourceGeneration(GeneratorExecutionContext context, FluentGeneratorContext fluentContext, FluentSetupSyntaxReceiver syntaxReceiver)
      {
         var fluentSetupClasses = fluentContext.FindFluentSetups(syntaxReceiver.SetupCandidates).ToArray();
         if (fluentSetupClasses.Length == 0)
            return;

         var fluentSetupModel = SetupModel.Create(fluentContext, fluentSetupClasses);
         var modelGenerator = new FluentModelGenerator();

         foreach (var generatedSource in modelGenerator.Execute(fluentSetupModel))
            AddSourceOrReportError(context, generatedSource);
      }

      #endregion
   }
}