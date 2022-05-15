// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenerationResult.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.Setups;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

internal class GenerationResult
{
   #region Constructors and Destructors

   public GenerationResult(Compilation compilation, IReadOnlyList<SyntaxTree> syntaxTrees)
   {
      Compilation = compilation ?? throw new ArgumentNullException(nameof(compilation));
      SyntaxTrees = syntaxTrees ?? throw new ArgumentNullException(nameof(syntaxTrees));
   }

   #endregion

   #region Public Properties

   public Compilation Compilation { get; }

   public ImmutableArray<Diagnostic> GeneratedDiagnostics { get; set; }

   public IReadOnlyList<SyntaxTree> SyntaxTrees { get; }

   #endregion

   #region Public Methods and Operators

   public void FailWith(string id, string hint)
   {
      if (!Compilation.GetDiagnostics().Any(d => d.Id == id))
         Assert.Fail($"The expected diagnostic {id} was not found. Hint : {hint}");
   }

   #endregion

   public void Print()
   {
      Debug.WriteLine(GetGeneratedCode());

      string GetGeneratedCode()
      {
         var builder = new StringBuilder();
         builder.AppendLine();
         builder.AppendLine("### GENERATED CODE ###");
         builder.AppendLine();

         foreach (var resultSyntaxTree in SyntaxTrees.Skip(1))
         {
            builder.AppendLine(resultSyntaxTree.ToString());
            builder.AppendLine();
            builder.AppendLine("".PadRight(50, '-'));
         }

         return builder.ToString();
      }
   }
}