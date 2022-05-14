// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultValueTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Tests
{
   using FluentAssertions;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class DefaultValueTests
   {
      [TestMethod]
      public void EnsureDefaultFieldValuesWorkCorrectly()
      {
         var target = Setup.DefaultField().Done();
         target.Age.Should().Be(10);
      }

      [TestMethod]
      public void EnsureDefaultPropertyValuesWorkCorrectly()
      {
         var target = Setup.DefaultProperty().Done();
         target.Age.Should().Be(20);
      }
   }

   [FluentSetup(typeof(DefaultTarget), SetupMethod = "DefaultField")]
   internal partial class DefaultField
   {
      [FluentMember]
      private int age = 10;
   }

   [FluentSetup(typeof(DefaultTarget), SetupMethod = "DefaultProperty")]
   internal partial class DefaultProperty
   {
      [FluentMember]
      private int Age { get; set; } = 20;
   }

   internal class DefaultTarget
   {
      public int Age { get; set; }
   }
}