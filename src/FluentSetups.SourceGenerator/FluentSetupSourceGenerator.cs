﻿namespace FluentSetups.SourceGenerator
{
   using System.Linq;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp.Syntax;

   [Generator]
   public class FluentSetupSourceGenerator : ISourceGenerator
   {

      public void Initialize(GeneratorInitializationContext context)
      {
         context.RegisterForSyntaxNotifications(() => new FluentSetupSyntaxReceiver());
      }

      public void Execute(GeneratorExecutionContext context)
      {
         if (!(context.SyntaxReceiver is FluentSetupSyntaxReceiver syntaxReceiver))
            return;

         Api = FluentApi.FromExecutionContext(context);
         if(Api.TryGetMissingType(out string missingType))
         {
            MissingReferenceDiagnostic(context, missingType);
            return;
         }
         
         GenerateSetups(context, Api.FindFluentSetups(syntaxReceiver.SetupCandidates).ToArray());
      }

      private FluentApi Api { get; set; }

      private void GenerateSetups(GeneratorExecutionContext context, ClassDeclarationSyntax[] fluentSetupClasses)
      {
         var generationRun = new GenerationRun(context);
         generationRun.Execute(fluentSetupClasses);
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

      private static readonly DiagnosticDescriptor InvalidXmlWarning = new DiagnosticDescriptor(id: "FS0001",
         title: "fluent setup class", messageFormat: "Starting generation for fluent setup class: '{0}'",
         category: nameof(FluentSetupSourceGenerator), DiagnosticSeverity.Info, isEnabledByDefault: true);

      private void GenerateClass(GeneratorExecutionContext context, ClassDeclarationSyntax candidate)
      {
         // context.ReportDiagnostic(Diagnostic.Create(InvalidXmlWarning, Location.None, candidate.Identifier));
         var generator = new FluentSetupClassGenerator(context, candidate);
         generator.Execute();
      }

   }
}
