namespace FluentSetups.SourceGenerator
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

         Api = FluentApi.FromCompilation(context.Compilation);
         if (Api.TryGetMissingType(out string missingType))
         {
            MissingReferenceDiagnostic(context, missingType);
            return;
         }

         var fluentSetupClasses = Api.FindFluentSetups(syntaxReceiver.SetupCandidates).ToArray();
         GenerateSetups(context, fluentSetupClasses);
      }

      private FluentApi Api { get; set; }

      private void GenerateSetups(GeneratorExecutionContext context, SetupClassInfo[] fluentSetupClasses)
      {
         if (fluentSetupClasses.Length == 0)
            return;

         var generationRun = new GenerationRun(context, Api);
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


   }
}
