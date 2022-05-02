﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupClassInfoSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.Setups;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

using FluentSetups.SourceGenerator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

internal class SetupClassInfoSetup
{
   #region Constants and Fields

   private readonly List<SyntaxTree> syntaxTrees = new(5);

   #endregion

   #region Public Properties

   public string ClassName { get; private set; }

   #endregion

   #region Public Methods and Operators

   public SetupClassInfoSetup AddSource(string code)
   {
      var syntaxTree = CSharpSyntaxTree.ParseText(code);
      var error = syntaxTree.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error).FirstOrDefault();
      if (error != null)
         throw new InvalidOperationException(error.GetMessage());

      syntaxTrees.Add(syntaxTree);
      return this;
   }

   public SetupClassInfo Done()
   {
      var compilation = CSharpCompilation.Create("FluentSetups.UnitTests.Compilation", syntaxTrees, ComputeReferences(),
         new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

      var outputCompilation = compilation;
      //var generator = new FluentSetupSourceGenerator();
      //var driver = CSharpGeneratorDriver.Create(generator);
      //driver.RunGeneratorsAndUpdateCompilation(compilation, out outputCompilation, out var generateDiagnostics);

      var syntaxWalker = new HelperSyntaxWalker();
      syntaxWalker.Visit(syntaxTrees[0].GetRoot());
      var setupClass = syntaxWalker.GetSetupClasses()[0];
      
      var api = FluentApi.FromCompilation(outputCompilation);
      var semanticModel = outputCompilation.GetSemanticModel(syntaxTrees[0]);
      return new SetupClassInfo(api, setupClass, semanticModel);
   }

   public SetupClassInfoSetup WithName(string className)
   {
      ClassName = className;
      return this;
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

   private static (ImmutableArray<Diagnostic>, string) RunSourceGenerator(string source)
   {
      var syntaxTree = CSharpSyntaxTree.ParseText(source);
      var FluentSetupsAssembly = typeof(FluentSetupAttribute).Assembly;
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

      var generator = new FluentSetupSourceGenerator();

      var driver = CSharpGeneratorDriver.Create(generator);
      driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generateDiagnostics);

      //// var immutableArray = compilation.GetDiagnostics();
      return (generateDiagnostics, outputCompilation.SyntaxTrees.Last().ToString());
   }

   #endregion
}