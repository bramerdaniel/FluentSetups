// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CtorArgsTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Tests;

using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class CtorArgsTests
{
   [TestMethod]
   public void EnsureTheConstructorIsCalledCorrectly()
   {
      var ctorArgs = Setup.CtorArgs()
         .WithFirst("Hans")
         .WithSecond(5)
         .WithThird(true)
         .Done();

      ctorArgs.First.Should().Be("Hans");
      ctorArgs.Second.Should().Be(5);
      ctorArgs.Third.Should().Be(true);
   }

   [TestMethod]
   public void EnsureMissingFirstConstructorArgumentThrows()
   {
      var setup = Setup.CtorArgs()
         .WithSecond(5)
         .WithThird(true);

      setup.Invoking(s => s.Done()).Should()
         .Throw<SetupMemberNotInitializedException>()
         .Where(e => e.MemberName == "first");
   }

   [TestMethod]
   public void EnsureMissingSecondConstructorArgumentThrows()
   {
      var setup = Setup.CtorArgs()
         .WithFirst("Hans")
         .WithThird(true);

      setup.Invoking(s => s.Done()).Should()
         .Throw<SetupMemberNotInitializedException>()
         .Where(e => e.MemberName == "second");
   }

   [TestMethod]
   public void EnsureMissingThirdConstructorArgumentThrows()
   {
      var setup = Setup.CtorArgs()
         .WithFirst("Hans")
         .WithSecond(5);

      setup.Invoking(s => s.Done()).Should()
         .Throw<SetupMemberNotInitializedException>()
         .Where(e => e.MemberName == "third");
   }


}