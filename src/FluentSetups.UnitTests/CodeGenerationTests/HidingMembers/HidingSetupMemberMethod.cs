// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HidingSetupMemberMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests.HidingMembers;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class HidingSetupMemberMethod
{
   [TestMethod]
   public void EnsureSetupMemberOfTargetPropertyCanBeHiddenByUserCode()
   {
      var code = @"using FluentSetups;

                   public class Person
                   {
                      public string Name { get ;set; }
                   }

                   [FluentSetup(typeof(Person))]
                   internal partial class PersonSetup
                   {
                       protected void SetupName(Person target)
                       {
                           target.Name = name;
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
   public void EnsureSetupMemberOfFluentPropertyCanBeHiddenByUserCode()
   {
      var code = @"using FluentSetups;

                   public class Person
                   {
                      public string Name { get ;set; }
                   }

                   [FluentSetup(typeof(Person))]
                   internal partial class PersonSetup
                   {
                       [FluentMember]
                       public string Name { get ;set; }

                       protected void SetupName(Person target)
                       {
                           target.Name = Name;
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
   public void EnsureSetupMemberOfFluentFieldCanBeHiddenByUserCode()
   {
      var code = @"using FluentSetups;

                   public class Person
                   {
                      public string Name { get ;set; }
                   }

                   [FluentSetup(typeof(Person))]
                   internal partial class PersonSetup
                   {
                       [FluentMember]
                       private string name;

                       protected void SetupName(Person target)
                       {
                           target.Name = name;
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