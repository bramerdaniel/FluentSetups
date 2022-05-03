// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntryPointGenerationTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class EntryPointGenerationTests
{
   #region Public Methods and Operators

   [TestMethod]
   public void EnsureSetupMethodIsGeneratedCorrectly()
   {
      string code = @"namespace RonnyTheRobber
                      {
                         [FluentSetups.FluentSetup(""FSetup"")]
                         public partial class Person
                         { }
                      }";

      var result = Setup.SourceGeneratorTest()
         .AddSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RonnyTheRobber.FSetup")
         .WithMethod("Person");
   }
   
   [TestMethod]
   public void EnsureMultipleSetupMethodsGetSameEntryClass()
   {
      string code = @"namespace RonnyTheRobber
                      {
                         [FluentSetups.FluentSetup(""FSetup"")]
                         public partial class PersonSetup
                         { }

                         [FluentSetups.FluentSetup(""FSetup"")]
                         public partial class BallSetup
                         { }
                      }";

      var result = Setup.SourceGeneratorTest()
         .AddSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RonnyTheRobber.FSetup")
         .WithMethod("Person")
         .WithMethod("Ball");
   }

   [TestMethod]
   public void EnsureUsingIsAddedCorrectly()
   {
      string target = @"namespace SomeOther.ModelNameSpace
                      {
                         public class Person
                         { 
                         }
                      }";

      string code = @"namespace SetupNameSpace
                      {
                         using SomeOther.ModelNameSpace;

                         [FluentSetups.FluentSetup]
                         public partial class PersonSetup : FluentSetups.IFluentSetup<Person>
                         {
                            internal partial Person CreateInstance() => new Person();
                         }
                      }";

      var result = Setup.SourceGeneratorTest()
         .AddSource(target)
         .AddSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("SetupNameSpace.Setup")
         .WithMethod("Person");
   }

   [TestMethod]
   public void EnsureSetupTypesFromDifferentNamespacesWorkCorrectly()
   {
      string target = @"namespace FirstNamespace
                      {
                         [FluentSetups.FluentSetup]
                         public partial class FirstSetup : FluentSetups.IFluentSetup<string>
                         {
                            internal partial string CreateInstance() => """";
                         }
                      }";

      string code = @"namespace SecondNamespace
                      {
                         [FluentSetups.FluentSetup]
                         public partial class SecondSetup : FluentSetups.IFluentSetup<string>
                         {
                            internal partial string CreateInstance() => """";
                         }
                      }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("RootNamespace")
         .AddSource(target)
         .AddSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("FirstNamespace.Setup")
         .WithMethod("First")
         .WithMethod("Second");
   }

   #endregion
}