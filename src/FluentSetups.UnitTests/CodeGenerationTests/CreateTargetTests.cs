// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateTargetTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class CreateTargetTests
{
   [TestMethod]
   public void EnsureCreateTargetMethodIsCreatedCorrectlyForTarget()
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
         .WhereMethod("CreateTarget")
         .IsProtected()
         .Contains("var target = new Person(GetName(null));");
   }

   [TestMethod]
   public void EnsureCreateTargetMethodIsCreatedCorrectlyTargetWithDefaultConstructor()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       public class Person
                       {
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
         .WhereMethod("CreateTarget")
         .IsProtected()
         .Contains("var target = new Person();");
   }
   
   [TestMethod]
   public void EnsureCreateTargetMethodIsCreatedCorrectlyTargetWithSmallerAccessibility()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       internal class Person
                       {
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
         .WhereMethod("CreateTarget")
         .IsPrivate()
         .Contains("var target = new Person();");
   }
}