namespace FluentSetups.SourceGenerator
{
   using System.Linq;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp;
   using Microsoft.CodeAnalysis.CSharp.Syntax;

   [Generator]
   public class FluentSetupSourceGenerator : ISourceGenerator
   {
      private INamedTypeSymbol fluentSetupAttribute;

      public void Initialize(GeneratorInitializationContext context)
      {
         context.RegisterForSyntaxNotifications(() => new FluentSetupSyntaxReceiver());
      }

      public void Execute(GeneratorExecutionContext context)
      {
         if (!(context.SyntaxReceiver is FluentSetupSyntaxReceiver syntaxReceiver))
            return;

         fluentSetupAttribute = context.Compilation.GetTypeByMetadataName("FluentSetups.FluentSetupAttribute");
         
         foreach (var candidate in syntaxReceiver.SetupCandidates.Where(candidate => IsSetupClass(context, candidate)))
            GenerateClass(context, candidate);
      }

      private static readonly DiagnosticDescriptor InvalidXmlWarning = new DiagnosticDescriptor(id: "FS0001",
         title: "fluent setup class", messageFormat: "Starting generation for fluent setup class: '{0}'",
         category: nameof(FluentSetupSourceGenerator), DiagnosticSeverity.Info, isEnabledByDefault: true);

      private void GenerateClass(GeneratorExecutionContext context, ClassDeclarationSyntax candidate)
      {
         context.ReportDiagnostic(Diagnostic.Create(InvalidXmlWarning, Location.None, candidate.Identifier));
         var generator = new FluentSetupClassGenerator(context, candidate);
         generator.Execute();
      }

      private bool IsSetupClass(GeneratorExecutionContext context, ClassDeclarationSyntax candidate)
      {
         var semanticModel = context.Compilation.GetSemanticModel(candidate.SyntaxTree);
         var classSymbol = (ITypeSymbol)semanticModel.GetDeclaredSymbol(candidate);
         if (classSymbol == null)
            return false;

         foreach (var attributeData in classSymbol.GetAttributes())
         {
            if (fluentSetupAttribute.Equals(attributeData.AttributeClass, SymbolEqualityComparer.Default))
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
