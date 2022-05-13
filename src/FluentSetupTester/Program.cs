// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetupTester
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.IO;
   using System.Linq;
   using System.Reflection;

   using ConsoLovers.ConsoleToolkit.Core;

   using FluentSetups;
   using FluentSetups.SourceGenerator;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp;

   class Program
   {
      #region Constants and Fields

      static readonly ConsoleProxy ConsoleProxy = new();

      #endregion

      #region Public Properties

      public static Assembly FluentSetupsAssembly { get; set; }

      #endregion

      #region Methods

      private static ConsoleColor GetColor(DiagnosticSeverity diagnosticSeverity)
      {
         switch (diagnosticSeverity)
         {
            case DiagnosticSeverity.Hidden:
            case DiagnosticSeverity.Info:
               return ConsoleColor.DarkGray;
            case DiagnosticSeverity.Warning:
               return ConsoleColor.Yellow;
            case DiagnosticSeverity.Error:
               return ConsoleColor.Red;
            default:
               throw new ArgumentOutOfRangeException(nameof(diagnosticSeverity), diagnosticSeverity, null);
         }
      }

      static void Main()
      {
         var syntaxTrees = ParseInput("FluentSetupTester.Setup.cs", "FluentSetupTester.ToolSetup.cs");

         var (diagnostics, output) = RunSourceGenerator(syntaxTrees);
         if (diagnostics.Length > 0)
         {
            ConsoleProxy.WriteLine("Diagnostics:");
            foreach (var diagnostic in diagnostics)
               WriteDiagnostic(diagnostic);

            ConsoleProxy.WriteLine();
            ConsoleProxy.WriteLine("Output:");
         }

         ConsoleProxy.WriteLine(output);
         ConsoleProxy.ReadLine();
      }

      private static IEnumerable<SyntaxTree> ParseInput(params string[] fileNames)
      {
         foreach (var fileName in fileNames)
         {
            var resourceStream = typeof(Program).Assembly.GetManifestResourceStream(fileName);
            if (resourceStream == null)
               throw new InvalidOperationException($"File {fileName} not found");

            using (var reader = new StreamReader(resourceStream))
               yield return CSharpSyntaxTree.ParseText(reader.ReadToEnd());
         }
      }

      private static (ImmutableArray<Diagnostic>, string) RunSourceGenerator(IEnumerable<SyntaxTree> syntaxTrees)
      {
         FluentSetupsAssembly = typeof(FluentSetupAttribute).Assembly;
         var references = new List<MetadataReference>();

         Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
         foreach (var assembly in assemblies)
         {
            if (!assembly.IsDynamic)
            {
               references.Add(MetadataReference.CreateFromFile(assembly.Location));
            }
         }

         var compilation = CSharpCompilation.Create("RootNamespace", syntaxTrees, references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

         WriteDiagnosticsBeforeSourceGeneration(compilation);

         ConsoleProxy.WriteLine();
         ConsoleProxy.WriteLine($"Starting {nameof(FluentSetupSourceGenerator)}");
         ConsoleProxy.WriteLine();

         var generator = new FluentSetupSourceGenerator();

         var driver = CSharpGeneratorDriver.Create(generator);
         driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generateDiagnostics);

         //// var immutableArray = compilation.GetDiagnostics();
         return (generateDiagnostics, outputCompilation.SyntaxTrees.ElementAt(2).ToString());
      }

      private static void WriteDiagnostic(Diagnostic diagnostic)
      {
         ConsoleProxy.WriteLine(diagnostic.GetMessage(), GetColor(diagnostic.Severity));
      }

      private static void WriteDiagnosticsBeforeSourceGeneration(CSharpCompilation compilation)
      {
         var compilationDiagnostics = compilation.GetDiagnostics();
         if (compilationDiagnostics.Length == 0)
            return;

         foreach (var diagnostic in compilationDiagnostics)
            WriteDiagnostic(diagnostic);
      }

      #endregion
   }
}