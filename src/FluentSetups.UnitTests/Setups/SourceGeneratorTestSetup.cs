// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourceGeneratorTestSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.Setups;

using FluentSetups.SourceGenerator;

using Microsoft.CodeAnalysis.CSharp;

internal class SourceGeneratorTestSetup : SetupBase
{
   
   #region Public Methods and Operators

   public SourceGeneratorTestSetup WithRootNamespace(string value)
   {
      RootNamespace = value;
      return this;
   }

   public SourceGeneratorTestSetup WithSource(string code)
   {
      AddSource(code);
      return this;
   }
   
   public GenerationResult Done()
   {
      var compilation = CreateCompilation();

      var generator = new FluentSetupSourceGenerator();
      var driver = CSharpGeneratorDriver.Create(generator);
      driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generatedDiagnostics);
      
      return new GenerationResult(outputCompilation, SyntaxTrees)
      {
         GeneratedDiagnostics = generatedDiagnostics,
      };
   }


   #endregion

   #region Methods



   #endregion
}