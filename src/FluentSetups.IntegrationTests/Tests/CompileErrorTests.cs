// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompileErrorTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Tests;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Test class that ensures that all generated member are available, otherwise it would not compile</summary>
[TestClass]
public class CompileErrorTests
{
   [TestMethod]
   public void EnsureSingleTargetPropertyIsGeneratedCorrectly()
   {
      var target = Setup.SinglePropTarget()
         .WithBrand("Brand")
         .Done();

      target.Should().NotBeNull();
   }

   [TestMethod]
   public void EnsureMultipleTargetPropertiesGenerateFluentMethods()
   {
      var target = Setup.MultiPropTarget()
         .WithName("Brand")
         .WithKind(3)
         .WithType(typeof(string))
         .Done();

      target.Should().NotBeNull();
   }

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