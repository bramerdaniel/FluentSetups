// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetModeTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.SetupClassInfoTests;

using FluentAssertions;

using FluentSetups.UnitTests.Setups;

using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class TargetModeTests
{
   [TestMethod]
   public void EnsureTargetModeIsComputedCorrectlyFromArgument()
   {
      string code = @"namespace UnitTests;
                      using FluentSetups;
                      
                      public class Person
                      {
                      }

                      [FluentSetup(typeof(Person), TargetMode=TargetMode.Disabled)]
                      public partial class PersonSetup
                      {
                      }";

      var setupClassInfo = Setup.SetupClassInfo()
         .WithSource(code)
         .Done();

      setupClassInfo.TargetMode.Should().NotBeNull();
      setupClassInfo.TargetMode.Kind.Should().Be(TypedConstantKind.Enum);

      Assert.IsNotNull(setupClassInfo.TargetMode.Value);
      setupClassInfo.TargetMode.Value.Should().BeOfType<int>().Which.Should().Be(1);
   }
}