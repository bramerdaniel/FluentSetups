// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetterGenerationTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class GetterGenerationTests
{
   [TestMethod]
   public void EnsureGetOrDefaultMethodIsGeneratedCorrectlyForFields()
   {
      var code = @"namespace DonnyTheDagger
                      {
                         using FluentSetups;

                         [FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentMember]
                            public int age;
                         }
                      }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("DonnyTheDagger.PersonSetup")
         .WithProtectedMethod("GetAgeOrDefault");
   }

   [TestMethod]
   public void EnsureGetOrThrowMethodsIsGeneratedCorrectlyForFields()
   {
      var code = @"namespace DonnyTheDagger
                      {
                         using FluentSetups;

                         [FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentMember]
                            public int age;
                         }
                      }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("DonnyTheDagger.PersonSetup")
         .WithProtectedMethod("GetAgeOrThrow");
   }

   [TestMethod]
   public void EnsureGetOrDefaultIsGeneratedCorrectlyForProperties()
   {
      var code = @"namespace RonnyTheRobber
                      {
                         [FluentSetups.FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentSetups.FluentMember]
                            public string Name { get; set; }
                         }
                      }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RonnyTheRobber.PersonSetup")
         .WithProtectedMethod("GetNameOrDefault");
   }

   [TestMethod]
   public void EnsureGetOrThrowMethodsIsGeneratedCorrectlyForProperties()
   {
      var code = @"namespace RonnyTheRobber
                      {
                         [FluentSetups.FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentSetups.FluentMember]
                            public string Name { get; set; }
                         }
                      }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RonnyTheRobber.PersonSetup")
         .WithProtectedMethod("GetNameOrThrow");
   }

   [TestMethod]
   public void EnsureNameWasSetFieldIsGeneratedForNameProperty()
   {
      var code = @"namespace RonnyTheRobber
                      {
                         [FluentSetups.FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentSetups.FluentMember]
                            public string Name { get; set; }
                         }
                      }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RonnyTheRobber.PersonSetup")
         .WithField("nameWasSet");
   }

   [TestMethod]
   public void EnsureNameWasSetFieldIsGeneratedForNameFields()
   {
      var code = @"namespace RonnyTheRobber
                      {
                         [FluentSetups.FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentSetups.FluentMember]
                            private string name;
                         }
                      }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RonnyTheRobber.PersonSetup")
         .WithField("nameWasSet");
   }
}