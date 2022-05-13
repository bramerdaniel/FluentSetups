// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetTypeTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.SetupClassInfoTests;

using System;

using FluentAssertions;

using FluentSetups.UnitTests.Setups;

using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class TargetTypeTests
{
   [TestMethod]
   public void EnsureTargetTypeNameIsComputedCorrectlyFromDefaultArgument()
   {
      string code = @"[FluentSetups.FluentSetup(typeof(System.Threading.Thread))]
                      public partial class PersonSetup
                      {
                      }";

      var setupClassInfo = Setup.SetupClassInfo()
         .WithSource(code)
         .Done();

      setupClassInfo.TargetType.Should().NotBeNull();
      setupClassInfo.TargetType.Kind.Should().Be(TypedConstantKind.Type);

      Assert.IsNotNull(setupClassInfo.TargetType.Value);
      setupClassInfo.TargetType.Value.ToString().Should().Be("System.Threading.Thread");
   }

   [TestMethod]
   public void EnsureTargetTypeNameIsComputedCorrectlyFromNamedArgument()
   {
      string code = @"[FluentSetups.FluentSetup(TargetType = typeof(string))]
                      public partial class PersonSetup
                      {
                      }";

      var setupClassInfo = Setup.SetupClassInfo()
         .WithSource(code)
         .Done();

      setupClassInfo.TargetType.Should().NotBeNull();
      setupClassInfo.TargetType.Kind.Should().Be(TypedConstantKind.Type);
     
      Assert.IsNotNull(setupClassInfo.TargetType.Value);
      setupClassInfo.TargetType.Value.ToString().Should().Be("string");
   }

   [TestMethod]
   public void EnsureCustomTargetTypeNameIsComputedCorrectlyFromNamedArgument()
   {
      string code = @"[FluentSetups.FluentSetup(TargetType = typeof(Person))]
                      public partial class PersonSetup
                      {
                      }

                      public class Person
                      {
                      }";

      var setupClassInfo = Setup.SetupClassInfo()
         .WithSource(code)
         .Done();

      setupClassInfo.TargetType.Should().NotBeNull();
      setupClassInfo.TargetType.Kind.Should().Be(TypedConstantKind.Type);

      Assert.IsNotNull(setupClassInfo.TargetType.Value);
      setupClassInfo.TargetType.Value.ToString().Should().Be("Person");
   }

   [TestMethod]
   public void EnsureUnknownTargetTypeNameIsComputedCorrectlyFromNamedArgument()
   {
      string code = @"[FluentSetups.FluentSetup(TargetType = typeof(Person))]
                      public partial class PersonSetup
                      {
                      }";

      var setupClassInfo = Setup.SetupClassInfo()
         .WithSource(code)
         .Done();

      setupClassInfo.TargetType.Should().NotBeNull();
      setupClassInfo.TargetType.Kind.Should().Be(TypedConstantKind.Type);

      Assert.IsNotNull(setupClassInfo.TargetType.Value);
      setupClassInfo.TargetType.Value.ToString().Should().Be("Person");
   }
}