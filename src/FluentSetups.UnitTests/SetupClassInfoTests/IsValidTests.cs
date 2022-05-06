// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsValidTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.SetupClassInfoTests;

using System;

using FluentAssertions;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class IsValidTests
{
   [TestMethod]
   public void EnsureDefaultSetupClassIsValid()
   {
      string code = @"[FluentSetups.FluentSetup(""MyCustomSetup"")]
                      public partial class PersonSetup : ISetup<string>
                      {
                         [FluentProperty]
                         public string Name { get; set; }

                      }";

      var setupClassInfo = Setup.SetupClassInfo()
         .WithSource(code)
         .Done();

      setupClassInfo.IsValidSetup().Should().BeTrue();
   }

   [TestMethod]
   public void EnsureIncorrectAttributeClassIsInvalid()
   {
      string code = @"[FluentSetup]
                      public partial class CodeSetup : ISetup<string>
                      {
                         [FluentProperty]
                         public string Name { get; set; }

                      }";

      Setup.SetupClassInfo()
         .WithSource(code)
         .Invoking(x => x.Done())
         .Should().Throw<ArgumentException>();
   }

}