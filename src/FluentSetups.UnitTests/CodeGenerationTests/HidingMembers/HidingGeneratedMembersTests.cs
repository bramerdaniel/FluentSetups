// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HidingGeneratedMembersTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests.HidingMembers;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class HidingGeneratedMembersTests
{
   [TestMethod]
   public void EnsureNoTargetPropertiesIsGeneratedWhenAlsoDefined()
   {
      var code = @"using FluentSetups;

                   public class Person
                   {
                       public string Name { get; set; }
                   }

                   [FluentSetup(typeof(Person))]
                   internal partial class PersonSetup
                   {
                       [FluentMember]
                       internal string Name { get; set; }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("Root")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("PersonSetup")
         .WithInternalMethod("WithName");

      result.Print();
   }

   [TestMethod]
   public void EnsureNoTargetSetupMethodIsGeneratedWhenAFieldAlsoDefined()
   {
      var code = @"using FluentSetups;

                   public class Person
                   {
                       public string Name { get; set; }
                   }

                   [FluentSetup(typeof(Person))]
                   internal partial class PersonSetup
                   {
                       [FluentMember]
                       private string name;
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("Root")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("PersonSetup")
         .WithInternalMethod("WithName");

      result.Print();
   }

   [TestMethod]
   public void EnsureNoSetupMethodIsGeneratedForTargetWhenDefinedByUser()
   {
      var code = @"using FluentSetups;

                   public class Person
                   {
                       public string Name { get; set; }
                   }

                   [FluentSetup(typeof(Person))]
                   internal partial class PersonSetup
                   {
                       internal PersonSetup WithName(string value)
                       {
                           return this;
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("Root")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("PersonSetup")
         .WithInternalMethod("WithName");
   }

   [TestMethod]
   public void EnsureSetupIsCreatedCorrectlyWhenOverloadExists()
   {
      var code = @"using FluentSetups;

                   public class Person
                   {
                       public string Name { get; set; }
                   }

                   [FluentSetup(typeof(Person))]
                   internal partial class PersonSetup
                   {
                       internal PersonSetup WithName(int value)
                       {
                           return this;
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("Root")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("PersonSetup")
         .WithMethod("WithName", "int")
         .WithMethod("WithName", "string");

      result.Print();
   }

   [TestMethod]
   public void EnsureNoDoneMethodIsGeneratedWhenUserDefinedOneExists()
   {
      var code = @"using FluentSetups;

                   public class Person
                   {
                   }

                   [FluentSetup(typeof(Person))]
                   internal partial class PersonSetup
                   {
                       internal Person Done()
                       {
                           return new Person();
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("Root")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("PersonSetup")
         .WithMethod("Done");
   }
   
   [TestMethod]
   public void EnsureDoneMethodOverloadIsPossible()
   {
      var code = @"using FluentSetups;

                   public class Person
                   {
                   }

                   [FluentSetup(typeof(Person))]
                   internal partial class PersonSetup
                   {
                       internal Person Done(bool forOverload)
                       {
                           return new Person();
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("Root")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("PersonSetup")
         .WithMethod("Done")
         .WithMethod("Done", "bool");
   }

   [TestMethod]
   public void EnsureCreateTargetCanBeHiddenByUserCode()
   {
      var code = @"using FluentSetups;

                   public class Person
                   {
                   }

                   [FluentSetup(typeof(Person))]
                   internal partial class PersonSetup
                   {
                       internal Person CreateTarget()
                       {
                           return new Person();
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("Root")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("PersonSetup")
         .WithMethod("CreateTarget")
         .WithMethod("Done");
   }
   
}