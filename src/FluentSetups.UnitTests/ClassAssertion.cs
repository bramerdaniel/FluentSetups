// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassAssertion.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests;

using System.Linq;

using FluentAssertions;
using FluentAssertions.Primitives;

using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

internal class ClassAssertion : ReferenceTypeAssertions<INamedTypeSymbol, ClassAssertion>
{
   #region Constructors and Destructors

   public ClassAssertion(INamedTypeSymbol subject)
      : base(subject)
   {
   }

   #endregion

   #region Properties

   protected override string Identifier => nameof(INamedTypeSymbol);

   #endregion

   #region Public Methods and Operators

   public ClassAssertion WithMethod(string methodName)
   {
      var methodSymbol = Subject.GetMembers(methodName).OfType<IMethodSymbol>().FirstOrDefault();
      Assert.IsNotNull(methodSymbol, $"The method {methodName} could not be found");
      return this;
   }

   public ClassAssertion WithProtectedMethod(string methodName)
   {
      var methodSymbol = Subject.GetMembers(methodName).OfType<IMethodSymbol>().FirstOrDefault();
      Assert.IsNotNull(methodSymbol, $"The method {methodName} could not be found");
      methodSymbol.DeclaredAccessibility.Should().Be(Accessibility.Protected);
      return this;
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