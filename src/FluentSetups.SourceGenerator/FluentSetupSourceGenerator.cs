namespace FluentSetups.SourceGenerator
{
   using System.Reflection;
   using System.Text;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp;
   using Microsoft.CodeAnalysis.CSharp.Syntax;
   using Microsoft.CodeAnalysis.Text;

   [Generator]
   public class FluentSetupSourceGenerator : ISourceGenerator
   {
      private INamedTypeSymbol fluentSetupAttribute;

      public void Initialize(GeneratorInitializationContext context) => 
         context.RegisterForSyntaxNotifications(() => new FluentSetupSyntaxReceiver());

      public void Execute(GeneratorExecutionContext context)
      {
         // GenerateApi(context);

         if (!(context.SyntaxReceiver is FluentSetupSyntaxReceiver syntaxReceiver))
            return;

         foreach (var candidate in syntaxReceiver.SetupCandidates)
         {
            if (IsSetupClass(context, candidate))
               GenerateClass(context, candidate);
         }
      }

      private void GenerateClass(GeneratorExecutionContext context, ClassDeclarationSyntax candidate)
      {
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
            var attributeClassName = attributeData.AttributeClass.Name;
            if (attributeClassName == "FluentSetup")
               return true;

            // TODO why does this not work
            if (fluentSetupAttribute.Equals(attributeData.AttributeClass, SymbolEqualityComparer.Default))
               return true;
         }

         return false;
      }

      private void GenerateApi(GeneratorExecutionContext context)
      {
         var assembly = GetType().Assembly;

         fluentSetupAttribute = AddAttribute(context, assembly, "FluentSetupAttribute");
         AddAttribute(context, assembly, "IFluentSetup");
      }

      private INamedTypeSymbol AddAttribute(GeneratorExecutionContext context, Assembly assembly, string attributeName)
      {
         using (var resourceStream = assembly.GetManifestResourceStream($"FluentSetups.{attributeName}.cs"))
         {
            if (resourceStream == null)
            {
               //context.ReportDiagnostic(Diagnostic.Create("Error", null, ); 
               return null;
            }

            var sourceText = SourceText.From(resourceStream, Encoding.UTF8);
            context.AddSource(attributeName, sourceText);

            //var options = context.Compilation.Options as CSharpCompilationOptions;
            //var syntaxTree = CSharpSyntaxTree.ParseText(sourceText);

            //var compiledTree = context.Compilation.AddSyntaxTrees(syntaxTree);
            //return compiledTree.GetTypeByMetadataName($"FluentSetups.{attributeName}");
            return null;
         }
      }
   }
}
