// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassAssertion.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests;

using System;
using System.Linq;
using System.Text;

using FluentAssertions;
using FluentAssertions.Primitives;

using FluentSetups.UnitTests.Setups;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

internal class ClassAssertion : ReferenceTypeAssertions<INamedTypeSymbol, ClassAssertion>
{
   private readonly GenerationResult generationResult;

   #region Constructors and Destructors

   public ClassAssertion(GenerationResult generationResult, INamedTypeSymbol subject)
      : base(subject)
   {
      this.generationResult = generationResult;
   }

   #endregion

   #region Properties

   protected override string Identifier => nameof(INamedTypeSymbol);

   #endregion

   #region Public Methods and Operators

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

   public ClassAssertion WithMethod(string methodName)
   {
      var methodSymbol = Subject.GetMembers(methodName).OfType<IMethodSymbol>().FirstOrDefault();
      Assert.IsNotNull(methodSymbol, $"The method {methodName} could not be found.{GetGeneratedCode()}");
      return this;
   }

   public ClassAssertion WithProtectedMethod(string methodName)
   {
      var methodSymbol = Subject.GetMembers(methodName).OfType<IMethodSymbol>().FirstOrDefault();
      Assert.IsNotNull(methodSymbol, $"The method {methodName} could not be found.{GetGeneratedCode()}");
      methodSymbol.DeclaredAccessibility.Should().Be(Accessibility.Protected);
      return this;
   }

   public ClassAssertion WithInternalMethod(string methodName)
   {
      var methodSymbol = Subject.GetMembers(methodName).OfType<IMethodSymbol>().FirstOrDefault();
      Assert.IsNotNull(methodSymbol, $"The method {methodName} could not be found");

      methodSymbol.DeclaredAccessibility.Should().Be(Accessibility.Internal);
      return this;
   }

   public ClassAssertion WithMethod(string methodName, params string[] parameterTypes)
   {
      var methods = Subject.GetMembers(methodName).OfType<IMethodSymbol>()
         .Where(x => x.Parameters.Length == parameterTypes.Length).ToArray();

      Assert.IsTrue(methods.Length > 0, $"The method {methodName} with {parameterTypes.Length} parameters could not be found");

      if (methods.Any(x => HasSignature(x, parameterTypes)))
         return this;

      Assert.Fail("Could not find a method with matching signature");
      return null;
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

   public ClassAssertion WithStaticMethod(string methodName)
   {
      var methodSymbol = Subject.GetMembers(methodName).OfType<IMethodSymbol>().FirstOrDefault();
      Assert.IsNotNull(methodSymbol, $"The method {methodName} could not be found");
      Assert.IsTrue(methodSymbol.IsStatic, $"The found method {methodName} is not static");
      return this;
   }

   public ClassAssertion WithoutMethod(string methodName)
   {
      var methodSymbol = Subject.GetMembers(methodName).OfType<IMethodSymbol>().FirstOrDefault();
      Assert.IsNull(methodSymbol, $"The method {methodName} was found but it should not.");
      return this;
   }

   #endregion

   public ClassAssertion MustBePartial()
   {
      var namedTypeSymbol = Subject;
      return this;
   }

   public ClassAssertion WithInternalModifier()
   {
      Subject.DeclaredAccessibility.Should().Be(Accessibility.Internal);
      return this;
   }

   public ClassAssertion WithPublicModifier()
   {
      Subject.DeclaredAccessibility.Should().Be(Accessibility.Public);
      return this;
   }
}