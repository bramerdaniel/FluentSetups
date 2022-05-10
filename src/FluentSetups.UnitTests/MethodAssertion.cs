﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodAssertion.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests;

using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

using FluentAssertions;
using FluentAssertions.Primitives;

using FluentSetups.UnitTests.Setups;

using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

internal class MethodAssertion : ReferenceTypeAssertions<IMethodSymbol, MethodAssertion>
{
   #region Constants and Fields

   private readonly GenerationResult generationResult;

   #endregion

   #region Constructors and Destructors

   public MethodAssertion(GenerationResult generationResult, IMethodSymbol subject)
      : base(subject)
   {
      this.generationResult = generationResult;
   }

   #endregion

   #region Properties

   protected override string Identifier => nameof(INamedTypeSymbol);

   #endregion

   #region Public Methods and Operators

   public MethodAssertion Contains(string expectedSubstring)
   {
      var syntaxReference = Subject.DeclaringSyntaxReferences.FirstOrDefault();
      if (syntaxReference == null)
         throw new AssertFailedException("Syntax reference could not be found");

      var code = syntaxReference.GetSyntax().ToString();
      Assert.IsTrue(code.Contains(expectedSubstring), CreateMessage());
      code.Should().Contain(expectedSubstring);
      
      return this;
      string CreateMessage()
      {
         var builder = new StringBuilder();
         builder.AppendLine($"The expected code '{expectedSubstring}' could not be found.");
         builder.AppendLine();
         builder.AppendLine("METHOD CODE");
         builder.AppendLine();
         builder.AppendLine(code);

         return builder.ToString();
      }
   }

   public MethodAssertion IsInternal()
   {
      Subject.DeclaredAccessibility.Should().Be(Accessibility.Internal);
      return this;
   }

   public MethodAssertion IsPublic()
   {
      Subject.DeclaredAccessibility.Should().Be(Accessibility.Public);
      return this;
   }

   #endregion

   #region Methods

   string GetGeneratedCode()
   {
      var builder = new StringBuilder();
      builder.AppendLine();
      builder.AppendLine("### INPUT ###");
      builder.AppendLine(generationResult.SyntaxTrees.First().ToString());
      builder.AppendLine();
      builder.AppendLine("### OUTPUT ###");

      foreach (var resultSyntaxTree in generationResult.SyntaxTrees.Skip(1))
      {
         builder.AppendLine(resultSyntaxTree.ToString());
         builder.AppendLine();
         builder.AppendLine("".PadRight(50, '-'));
      }

      return builder.ToString();
   }

   private bool HasSignature(IMethodSymbol methodSymbol, string[] parameterTypes)
   {
      var typeNames = methodSymbol.Parameters.Select(x => x.Type.ToString()).ToArray();
      for (var i = 0; i < parameterTypes.Length; i++)
      {
         var expectedType = parameterTypes[i];
         if (!string.Equals(expectedType, typeNames[i]))
            return false;
      }

      return true;
   }

   #endregion
}