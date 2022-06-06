// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateTargetTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests;

using System.Diagnostics;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class CreateTargetTests
{
#if NET6_0

   [TestMethod]
   public void EnsureRecordsAreSetupCorrectly()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;
                       using System.Collections.Generic;

                       public record Bag(IEnumerable<string> Values);
   
                       [FluentSetup(typeof(Bag))]
                       public partial class BagSetup
                       {
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.BagSetup")
         .WhereMethod("CreateTarget")
         .IsProtected()
         .Contains("var target = new Bag(GetValues());");

      result.Print();
   } 

#endif
   
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
         .Contains("var target = new Person(GetName());");

      result.Print();
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

      result.Print();
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

      result.Print();
   }
}