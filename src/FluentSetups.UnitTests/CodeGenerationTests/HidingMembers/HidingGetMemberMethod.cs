// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HidingGetMemberMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests.HidingMembers;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class HidingGetMemberMethod
{
   [TestMethod]
   public void EnsureGetMemberOfTargetPropertyCanBeHiddenByUserCode()
   {
      var code = @"using FluentSetups;

                   public class Person
                   {
                      public string Name { get ;set; }
                   }

                   [FluentSetup(typeof(Person))]
                   internal partial class PersonSetup
                   {
                       protected string GetName()
                       {
                           return name;
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("Root")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("PersonSetup")
         .WithMethod("SetupName");

      result.Print();
   }

   [TestMethod]
   public void EnsureGetMemberWithDefaultOfTargetPropertyCanBeHiddenByUserCode()
   {
      var code = @"using FluentSetups;
                   using System;

                   public class Person
                   {
                      public string Name { get ;set; }
                   }

                   [FluentSetup(typeof(Person))]
                   internal partial class PersonSetup
                   {
                       protected string GetName(Func<string> defaultValue)
                       {
                           return name ?? defaultValue();
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("Root")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("PersonSetup")
         .WithMethod("SetupName");

      result.Print();
   }
}