// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateTargetTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class CreateTargetTests : VerifyBase
{
#if NET6_0

   [TestMethod]
   public void EnsureRecordsAreSetupCorrectly()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;
                       using System.Collections.Generic;

                       public record Bag(IList<string> Values);
   
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

   [TestMethod]
   public async Task EnsureRecordsWithEnumerableWorkCorrectly()
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

      result.Should().NotHaveErrors();

      var withValuesCode = await result.Should()
         .HaveClass("MyTests.BagSetup")
         .WhereMethod("WithValues")
         .GetCodeAsync();
      await Verify(withValuesCode).UseMethodName("Record.Enumerable.WithValues");

      var withValueCode = await result.Should()
         .HaveClass("MyTests.BagSetup")
         .WhereMethod("WithValue")
         .GetCodeAsync();
      await Verify(withValueCode).UseMethodName("Record.Enumerable.WithValue");

      result.Print();
   }

   [TestMethod]
   public async Task EnsureRecordsWithEnumerableOfPeopleWorkCorrectly()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;
                       using System.Collections.Generic;
                       using SomeNamespace;

                       public record Bag(IEnumerable<Person> people);
   
                       [FluentSetup(typeof(Bag))]
                       public partial class BagSetup
                       {
                       }
                   }";

      var personCode = @"namespace SomeNamespace
                         {
                             public class Person { }
                         }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .WithSource(personCode)
         .Done();

      result.Should().NotHaveErrors();

      var withValuesCode = await result.Should()
         .HaveClass("MyTests.BagSetup")
         .WhereMethod("WithPeople")
         .GetCodeAsync();
      await Verify(withValuesCode).UseMethodName("Record.Enumerable.WithPeople");

      var withValueCode = await result.Should()
         .HaveClass("MyTests.BagSetup")
         .WhereMethod("WithPerson")
         .GetCodeAsync();
      await Verify(withValueCode).UseMethodName("Record.Enumerable.WithPerson");

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