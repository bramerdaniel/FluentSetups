// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetConstructionTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class TargetConstructionTests
{
   #region Public Methods and Operators

   [TestMethod]
   [Ignore]
   public void EnsureConstructorCallIsGeneratedCorrectly()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       public class Person
                       {
                           public Person(string name)
                           {
                               Name = name;
                           }

                           public string Name { get; set; }
                       }
   
                       [FluentSetup(typeof(Person))]
                       public partial class PersonSetup
                       {
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();
      
      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WithMethod("Done");
   }
   
   [TestMethod]
   [Ignore]
   public void EnsureConstructorCallWithTwoArgumentsIsGeneratedCorrectly()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       public class Person
                       {
                           public Person(string firstName, string lastName)
                           {
                               FirstName = firstName;
                               LastName = lastName;
                           }

                           public string FirstName { get; set; }

                           public string LastName { get; set; }
                       }
   
                       [FluentSetup(typeof(Person))]
                       public partial class PersonSetup
                       {
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();
      
      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WithMethod("Done");
   }


   [TestMethod]
   [Ignore]
   public void EnsureConstructorCallIsGeneratedCorrectlyForPrivateSetterProperties()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       public class Person
                       {
                           public Person(string name)
                           {
                               Name = name;
                           }

                           public string Name { get; }
                       }
   
                       [FluentSetup(typeof(Person))]
                       public partial class PersonSetup
                       {
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WithMethod("Done");
   }

   #endregion
}