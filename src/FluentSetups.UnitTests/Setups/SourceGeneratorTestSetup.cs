// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourceGeneratorTestSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.Setups;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentSetups.SourceGenerator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

internal class SourceGeneratorTestSetup
{
   #region Constants and Fields

   private readonly List<SyntaxTree> syntaxTrees = new(5);

   private string rootNamespace = "FluentSetups.UnitTests.Compilation";

   private bool throwOnErrors = true;

   #endregion

   #region Public Properties

   public string ClassName { get; private set; }

   #endregion

   #region Public Methods and Operators

   public SourceGeneratorTestSetup WithRootNamespace(string value)
   {
      rootNamespace = value;
      return this;
   }

   public SourceGeneratorTestSetup AddSource(string code)
   {
      var syntaxTree = CSharpSyntaxTree.ParseText(code);


      syntaxTrees.Add(syntaxTree);
      return this;
   }

   private void ThrowOnErrors(IEnumerable<Diagnostic> diagnostics)
   {
      var errorDiagnostic = diagnostics.FirstOrDefault(x => x.Severity == DiagnosticSeverity.Error);
      if (errorDiagnostic != null)
         throw new InvalidOperationException(errorDiagnostic.GetMessage());
   }

   public GenerationResult Done()
   {
      var compilation = CSharpCompilation.Create(rootNamespace, syntaxTrees, ComputeReferences(),
         new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

      var generator = new FluentSetupSourceGenerator();
      var driver = CSharpGeneratorDriver.Create(generator);
      driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generatedDiagnostics);
      
      return new GenerationResult(outputCompilation, syntaxTrees)
      {
         GeneratedDiagnostics = generatedDiagnostics,
      };
   }


   #endregion

   #region Methods

   private static List<MetadataReference> ComputeReferences()
   {
      var fluentSetupsAssembly = typeof(FluentSetupAttribute).Assembly;

      var references = new List<MetadataReference>();
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      foreach (var assembly in assemblies)
         if (!assembly.IsDynamic)
            references.Add(MetadataReference.CreateFromFile(assembly.Location));
      return references;
   }

   #endregion

   public SourceGeneratorTestSetup AllowErrors()
   {
      throwOnErrors = false;
      return this;
   }
}