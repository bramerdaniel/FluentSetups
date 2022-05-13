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
         .WhereMethod("Done")
         .Contains("var target = CreateTarget();")
         .Contains("SetupTarget(target);")
         .Contains("return target;");
   }
   
   [TestMethod]
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

   [TestMethod]
   public void EnsureCreateTargetMethodIsCreated()
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
         .WithMethod("CreateTarget")
         .WhereMethod("CreateTarget").Contains("var target = new Person(GetName(null));");
   }
   
   [TestMethod]
   public void EnsureSetupTargetMethodIsCreated()
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
         .WhereMethod("SetupTarget")
         .IsProtected();
   }

   [TestMethod]
   public void EnsureSetupTargetMethodIsCreatedPrivateWhenTargetIsInternalButSetupPublic()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       internal class Person
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
         .WhereMethod("SetupTarget")
         .IsPrivate();
   }
   
   [TestMethod]
   public void EnsureSetupTargetMethodIsCreatedProtectedWhenTargetAndSetupIsInternal()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       internal class Person
                       {
                           public Person(string name)
                           {
                               Name = name;
                           }

                           public string Name { get; }
                       }
   
                       [FluentSetup(typeof(Person))]
                       internal partial class PersonSetup
                       {
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WhereMethod("SetupTarget")
         .IsProtected();
   }

   #endregion
}