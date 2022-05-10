// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FPropertyAssertion.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.Assertions;

using FluentAssertions;
using FluentAssertions.Primitives;

using FluentSetups.SourceGenerator.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

internal class FPropertyAssertion : ReferenceTypeAssertions<FProperty, FPropertyAssertion>
{
   #region Constructors and Destructors

   public FPropertyAssertion(FProperty subject)
      : base(subject)
   {
   }

   #endregion

   #region Properties

   protected override string Identifier => nameof(FPropertyAssertion);

   #endregion

   #region Public Methods and Operators

   public AndConstraint<FPropertyAssertion> WithSetupMethodName(string expectedName)
   {
      Assert.AreEqual(expectedName, Subject.GetSetupMethodName());
      return new AndConstraint<FPropertyAssertion>(this);
   }

   #endregion

   public AndConstraint<FPropertyAssertion> WithTypeName(string expectedTypeName)
   {
      Assert.AreEqual(expectedTypeName, Subject.TypeName);
      return new AndConstraint<FPropertyAssertion>(this);
   }

   public AndConstraint<FPropertyAssertion> WithRequiredNamespace(string expectedNamespace)
   {
      Assert.AreEqual(expectedNamespace, Subject.RequiredNamespace);
      return new AndConstraint<FPropertyAssertion>(this);
   }

}