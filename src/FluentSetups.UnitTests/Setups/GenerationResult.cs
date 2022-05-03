// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenerationResult.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.Setups;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

internal class GenerationResult
{
   public Compilation Compilation { get; }

   public IReadOnlyList<SyntaxTree> SyntaxTrees { get; }

   public ImmutableArray<Diagnostic> GeneratedDiagnostics { get; set; }
   
   public GenerationResult(Compilation compilation, IReadOnlyList<SyntaxTree> syntaxTrees)
   {
      Compilation = compilation ?? throw new ArgumentNullException(nameof(compilation));
      SyntaxTrees = syntaxTrees ?? throw new ArgumentNullException(nameof(syntaxTrees));
   }

   public void FailWith(string id, string hint)
   {
      if (!Compilation.GetDiagnostics().Any(d => d.Id == id))
         Assert.Fail($"The expected diagnostic {id} was not found. Hint : {hint}");
   }
}