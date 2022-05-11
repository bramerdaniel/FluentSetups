// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CarTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Tests;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class CarTests
{
   [TestMethod]
   [Ignore]
   public void CreateCar()
   {
      var car = Setup.SinglePropTarget()
         .WithBrand("BMW")
         .Done();

      car.Brand.Should().Be("BMW");
   }


}