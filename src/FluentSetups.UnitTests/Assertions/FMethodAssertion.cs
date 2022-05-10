// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FMethodAssertion.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.Assertions;

using FluentAssertions;
using FluentAssertions.Primitives;

using FluentSetups.SourceGenerator.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

internal class FMethodAssertion : ReferenceTypeAssertions<FMethod, FMethodAssertion>
{
   #region Constructors and Destructors

   public FMethodAssertion(FMethod subject)
      : base(subject)
   {
   }

   #endregion

   #region Properties

   protected override string Identifier => nameof(FMethodAssertion);

   #endregion

   #region Public Methods and Operators


   public AndConstraint<FMethodAssertion> WithTypeName(string expectedTypeName)
   {
      Assert.AreEqual(expectedTypeName, Subject.ParameterTypeName);
      return new AndConstraint<FMethodAssertion>(this);
   }

   public AndConstraint<FMethodAssertion> WithReturnTypeName(string expectedTypeName)
   {
      Assert.AreEqual(expectedTypeName, Subject.ReturnType);
      return new AndConstraint<FMethodAssertion>(this);
   }

   #endregion
}