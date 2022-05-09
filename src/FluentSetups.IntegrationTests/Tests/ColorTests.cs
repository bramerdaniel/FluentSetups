// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Tests;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ColorTests
{
   [TestMethod]
   public void EnsureOverloadExists()
   {
      var car = Setup.Color()
         .WithName("Name")
         .WithOpacity(5)
         .WithOpacity("7")
         .Done();

      car.Should().NotBeNull();
   }


}