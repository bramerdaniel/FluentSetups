// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssertionExtensions.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests;

using FluentSetups.UnitTests.Setups;

internal static class AssertionExtensions
{
   #region Public Methods and Operators

   public static GenerationResultAssertion Should(this GenerationResult target)
   {
      return new GenerationResultAssertion(target);
   }

   #endregion
}