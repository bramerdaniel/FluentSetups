// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Tests;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class PersonTests
{
   [TestMethod]
   public void SetupThatIsHidingAPublicProperty()
   {
      var person = Setup.Person()
         .WithFirstName("Robert")
         .WithLastName("Ramirez")
         .WithAge(34) 
         .Done();

      person.FirstName.Should().Be("Robert");
      person.LastName.Should().Be("Ramirez");
      person.Age.Should().Be(34);
   }

   [TestMethod]
   public void SetupPersonWithDefaultName()
   {
       var person = Setup.PersonWithDefaultName()
           .Done();

       person.FirstName.Should().Be("John");
       person.LastName.Should().Be("Doe");
   }

   [TestMethod]
   public void SetupPersonWithCustomDefaultName()
   {
       var person = Setup.Person()
           .WithDefaults()
           .Done();

       person.FirstName.Should().Be("Lila");
       person.LastName.Should().Be("Sheer");
   }
}
