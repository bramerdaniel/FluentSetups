// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetupTester
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Linq;
   using System.Reflection;

   using ConsoLovers.ConsoleToolkit.Core;

   using FluentSetups;
   using FluentSetups.SourceGenerator;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp;

   class Program
   {
      #region Methods

      private static (ImmutableArray<Diagnostic>, string) GetGeneratedOutput(string source)
      {
         new FluentSetupAttribute();
         var syntaxTree = CSharpSyntaxTree.ParseText(source);

         var references = new List<MetadataReference>();
         Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
         foreach (var assembly in assemblies)
         {
            if (!assembly.IsDynamic)
            {
               references.Add(MetadataReference.CreateFromFile(assembly.Location));
            }
         }

         var compilation = CSharpCompilation.Create("foo", new[] { syntaxTree }, references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

         var compilationDiagnostics = compilation.GetDiagnostics();
         foreach (var diagnostic in compilationDiagnostics)
         {
            WriteDiagnostic(diagnostic);
         }
         //if (compilationDiagnostics.Any())
         //   return (compilationDiagnostics, "");

         var generator = new FluentSetupSourceGenerator();

         var driver = CSharpGeneratorDriver.Create(generator);
         driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generateDiagnostics);

         var immutableArray = compilation.GetDiagnostics();
         return (generateDiagnostics, outputCompilation.SyntaxTrees.Last().ToString());
      }

      private static void WriteDiagnostic(Diagnostic diagnostic)
      {
         var consoleProxy = new ConsoleProxy();
         consoleProxy.WriteLine(diagnostic.GetMessage(), GetColor(diagnostic.Severity));
      }

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

      static void Main(string[] args)
      {
         string source = @"
namespace Foo.Demo.Namespace
{
   using FluentSetups;
   
   class MyBase
   {
   }

   [FluentSetup]
   public partial class ToolSetup : IFluentSetup<string>
   {
      [FluentProperty]
      public int Number { get; set; }

      [FluentProperty]
      public string Name { get; set; }

      internal string CreateInstance()
      {
         return $""{Name} => {Number}"";
      }
   }
}
";
         var (diagnostics, output) = GetGeneratedOutput(source);

         if (diagnostics.Length > 0)
         {
            Console.WriteLine("Diagnostics:");
            foreach (var diag in diagnostics)
            {
               Console.WriteLine($"   {diag}");
            }

            Console.WriteLine();
            Console.WriteLine("Output:");
         }

         Console.WriteLine(output);
      }

      #endregion
   }
}