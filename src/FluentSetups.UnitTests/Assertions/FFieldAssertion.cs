// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FFieldAssertion.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.Assertions;

using FluentAssertions;
using FluentAssertions.Primitives;

using FluentSetups.SourceGenerator.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

internal class FFieldAssertion : ReferenceTypeAssertions<FField, FFieldAssertion>
{
   #region Constructors and Destructors

   public FFieldAssertion(FField subject)
      : base(subject)
   {
   }

   #endregion

   #region Properties

   protected override string Identifier => nameof(FFieldAssertion);

   #endregion

   #region Public Methods and Operators

   public AndConstraint<FFieldAssertion> WithRequiredNamespace(string expectedNamespace)
   {
      Assert.AreEqual(expectedNamespace, Subject.RequiredNamespace , $"The required namespace {expectedNamespace} was not found.");
      return new AndConstraint<FFieldAssertion>(this);
   }

   public AndConstraint<FFieldAssertion> WithSetupMethodName(string expectedName)
   {
      Assert.AreEqual(expectedName, Subject.SetupMethodName);
      return new AndConstraint<FFieldAssertion>(this);
   }

   public AndConstraint<FFieldAssertion> WithTypeName(string expectedTypeName)
   {
      Assert.AreEqual(expectedTypeName, Subject.TypeName);
      return new AndConstraint<FFieldAssertion>(this);
   }

   #endregion
}