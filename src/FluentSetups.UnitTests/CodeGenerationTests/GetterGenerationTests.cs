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
         .WithProtectedMethod("GetAge");
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
         .WhereMethod("GetAge")
         .IsProtected()
         .Contains("return GetAge(() => throw new SetupMemberNotInitializedException(nameof(age)));");
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
         .WithProtectedMethod("GetName");
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
         .WhereMethod("GetName")
         .IsProtected()
         .Contains("return GetName(() => throw new SetupMemberNotInitializedException(nameof(Name)));");
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

   [TestMethod]
   public void EnsureGetNameIsGeneratedFromTargetProperty()
   {
      var code = @"namespace RonnyTheRobber
                      {
                         internal class Person 
                         {  
                            public string Name{ get; set; }
                         }

                         [FluentSetups.FluentSetup(typeof(Person))]
                         public partial class PersonSetup
                         {
                         }
                      }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RonnyTheRobber.PersonSetup")
         .WithField("nameWasSet")
         .WhereMethod("GetName").IsProtected();
   }

   [TestMethod]
   public void EnsureGetNameIsGeneratedFromTargetConstructorParameter()
   {
      var code = @"namespace RonnyTheRobber
                      {
                         internal class Person 
                         {  
                           public Person(string name) 
                           {
                           }
                         }

                         [FluentSetups.FluentSetup(typeof(Person))]
                         public partial class PersonSetup
                         {
                         }
                      }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RonnyTheRobber.PersonSetup")
         .WithField("nameWasSet")
         .WhereMethod("GetName").IsProtected();
   }

   [TestMethod]
   public void EnsureContentOfGetNameIsGeneratedCorrectly()
   {
      var code = @"namespace RonnyTheRobber
                      {
                         internal class Person 
                         {  
                           public Person(string name) 
                           {
                           }
                         }

                         [FluentSetups.FluentSetup(typeof(Person))]
                         public partial class PersonSetup
                         {
                         }
                      }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RonnyTheRobber.PersonSetup")
         .WhereMethod("GetName")
         .Contains("return GetName(() => throw new SetupMemberNotInitializedException(nameof(name)));");
   }
}