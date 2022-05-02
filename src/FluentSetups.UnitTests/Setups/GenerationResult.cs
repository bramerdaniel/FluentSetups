// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenerationResult.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.Setups;

using System;
using System.Collections.Generic;

using Microsoft.CodeAnalysis;

internal class GenerationResult
{
   public Compilation Compilation { get; }

   public IList<SyntaxTree> SyntaxTrees { get; }

   public GenerationResult(Compilation compilation, IList<SyntaxTree> syntaxTrees)
   {
      Compilation = compilation ?? throw new ArgumentNullException(nameof(compilation));
      SyntaxTrees = syntaxTrees ?? throw new ArgumentNullException(nameof(syntaxTrees));
   }
}