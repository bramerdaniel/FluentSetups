﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentSetupSourceGenerator.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
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
         foreach (var generatedSource in new FluentModelGenerator().Execute(fluentSetupModel))
            context.AddSource(generatedSource.Name, SourceText.From(generatedSource.Code, Encoding.UTF8));
      }

      #endregion
   }

   internal class GeneratedSource
   {
      public string Name { get; set; }
      public string Code { get; set; }
   }
}