// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OverrideDoneTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Tests;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class OverrideDoneTests
{
   [TestMethod]
   public void EnsureOverwrittenDoneCompiles()
   {
      var person = CustomSetup.Person()
         .WithFirstName("Robert")
         .WithLastName("Ramirez")
         .Done();

      person.FirstName.Should().Be("Ramirez");
      person.LastName.Should().Be("Robert");
      person.Age.Should().Be(10);
   }
}