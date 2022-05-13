// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetMemberTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class SetMemberTests
{
   #region Public Methods and Operators
   
   [TestMethod]
   public void EnsureSetMemberIsGeneratedCorrectlyForTargetProperties()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       public class Person
                       {
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
         .WhereMethod("SetupName")
         .HasParameter("Person target");

      result.Print();
   }

   [TestMethod]
   public void EnsureSetMemberIsGeneratedCorrectlyForUserDefinedProperties()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       public class Person
                       {
                           public string Name { get; set; }
                       }
   
                       [FluentSetup(typeof(Person))]
                       public partial class PersonSetup
                       {
                           [FluentMember]
                           public string Name { get; set;  }
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WhereMethod("SetupName")
         .HasParameter("Person target")
         .Contains("target.Name = Name");

      result.Should()
         .HaveClass("MyTests.PersonSetup")
         .WhereMethod("SetupTarget")
         .Contains("SetupName(target);");

      result.Print();
   }

   [TestMethod]
   public void EnsureNoSetMemberWhenPropertyDoesNotExistsOnTarget()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       public class Person
                       {
                          // The person has no Name property   
                       }
   
                       [FluentSetup(typeof(Person))]
                       public partial class PersonSetup
                       {
                           [FluentMember]
                           private string name;
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WithoutMethod("SetupName");

      result.Print();
   }

   [TestMethod]
   public void EnsureSetMemberIsGeneratedCorrectlyForUserDefinedFields()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       public class Person
                       {
                           public string Name { get; set; }
                       }
   
                       [FluentSetup(typeof(Person))]
                       public partial class PersonSetup
                       {
                           [FluentMember]
                           private string name;
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WhereMethod("SetupName")
         .HasParameter("Person target")
         .Contains("target.Name = name;");

      result.Print();
   }
  
   [TestMethod]
   public void EnsureNoSetMemberWhenPropertyOnTargetIsPrivate()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       public class Person
                       {
                           private string Name { get; set; }
                       }
   
                       [FluentSetup(typeof(Person))]
                       public partial class PersonSetup
                       {
                           [FluentMember]
                           private string name;
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WithoutMethod("SetupName");

      result.Print();
   }

   [TestMethod]
   public void EnsureNoSetMemberWhenPropertyOnTargetHasPrivateSetter()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       public class Person
                       {
                           public string Name { get; private set; }
                       }
   
                       [FluentSetup(typeof(Person))]
                       public partial class PersonSetup
                       {
                           [FluentMember]
                           private string name;
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WithoutMethod("SetupName");

      result.Print();
   }
   
   [TestMethod]
   public void EnsureNoSetMemberWhenPropertyOnTargetHasNoSetter()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       public class Person
                       {
                           public string Name { get; }
                       }
   
                       [FluentSetup(typeof(Person))]
                       public partial class PersonSetup
                       {
                           [FluentMember]
                           private string name;
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WithoutMethod("SetName");

      result.Print();
   }

   [TestMethod]
   public void EnsureSetNameIsGeneratedCorrectlyForInternalTargetClass()
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
         .WhereMethod("SetupName")
         .IsPrivate();

      result.Print();
   }

   [TestMethod]
   public void EnsureSetNameIsGeneratedCorrectlyForInternalTargetAndInternalSetupClass()
   {
      var code = @"namespace RonnyTheRobber
                      {
                         internal class Person 
                         {  
                            public string Name{ get; set; }
                         }

                         [FluentSetups.FluentSetup(typeof(Person))]
                         internal partial class PersonSetup
                         {
                         }
                      }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RonnyTheRobber.PersonSetup")
         .WhereMethod("SetupName")
         .IsProtected();

      result.Print();
   }

   #endregion
}