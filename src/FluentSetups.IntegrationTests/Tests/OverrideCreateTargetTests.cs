// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OverrideCreateTargetTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Tests;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class OverrideCreateTargetTests
{
   [TestMethod]
   public void EnsureOverwrittenDoneCompiles()
   {
      var setup = Setup.CreateTargetOverwrite()
         .WithFirstName("Robert")
         .WithLastName("Ramirez");

      setup.CreateTargetCalled.Should().BeFalse();

      var person = setup.Done();

      setup.CreateTargetCalled.Should().BeTrue();
      person.FirstName.Should().Be("Robert");
      person.LastName.Should().Be("Ramirez");
      person.Age.Should().Be(10);
   }
}