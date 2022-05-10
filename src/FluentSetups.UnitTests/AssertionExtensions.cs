// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssertionExtensions.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests;

using FluentSetups.SourceGenerator.Models;
using FluentSetups.UnitTests.Assertions;
using FluentSetups.UnitTests.Setups;

internal static class AssertionExtensions
{
   #region Public Methods and Operators

   public static GenerationResultAssertion Should(this GenerationResult target)
   {
      return new GenerationResultAssertion(target);
   }

   public static SetupClassModelAssertion Should(this FClass target)
   {
      return new SetupClassModelAssertion(target);
   }

   #endregion
}