// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultValueTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class DefaultValueTests
{
   [TestMethod]
   public void EnsureSetupNameIsGeneratedCorrectlyForFieldsWithDefaultValues()
   {
      var code = @"namespace RonnyTheRobber
                   {   
                      using FluentSetups;

                      internal class Person 
                      {  
                         public string Name{ get; set; }
                      }

                      [FluentSetup(typeof(Person))]
                      internal partial class PersonSetup
                      {
                          [FluentMember]
                          private string name = ""Robert"";
                      }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RonnyTheRobber.PersonSetup")
         .WhereMethod("SetupName")
         .IsProtected()
         .Contains("target.Name = name;")
         .NotContains("nameWasSet");

      result.Print();
   }

   [TestMethod]
   public void EnsureSetupDoesNotCauseWarningsForUnusedFields()
   {
      var code = @"namespace RonnyTheRobber
                   {   
                      using FluentSetups;

                      internal class Person 
                      {  
                         public string Name{ get; set; }
                      }

                      [FluentSetup(typeof(Person))]
                      internal partial class PersonSetup
                      {
                          [FluentMember]
                          private string name = ""Robert"";
                      }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveWarnings();

      result.Print();
   }

   [TestMethod]
   public void EnsureSetupNameIsGeneratedCorrectlyForPropertiesWithDefaultValues()
   {
      var code = @"namespace RonnyTheRobber
                   {   
                      using FluentSetups;

                      internal class Person 
                      {  
                         public string Name { get; set; }
                      }

                      [FluentSetup(typeof(Person))]
                      internal partial class PersonSetup
                      {
                          [FluentMember]
                          private string Name { get; set; } = ""Robert"";
                      }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RonnyTheRobber.PersonSetup")
         .WhereMethod("SetupName")
         .IsProtected()
         .Contains("target.Name = Name;")
         .NotContains("nameWasSet");

      result.Print();
   }

   [TestMethod]
   public void EnsureCreateTargetMethodIsCreatedCorrectlyForTarget()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;
   
                       [FluentSetup]
                       public partial class PersonSetup
                       {
                           [FluentMember]
                           private int number = 123;
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WhereMethod("GetNumber")
         .IsProtected()
         .Contains("return number;");
   }

   [TestMethod]
   public void EnsureGetNumberMethodIsCreatedCorrectlyForTargetProperties()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;
   
                       [FluentSetup]
                       public partial class PersonSetup
                       {
                           [FluentMember]
                           public int Number { get; set; } = 123;
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WhereMethod("GetNumber")
         .IsProtected()
         .Contains("return Number;");


      result.Should().HaveClass("MyTests.PersonSetup")
         .WithoutMethod("GetNumber" ,"System.Func<int>");
   }
   
   [TestMethod]
   public void EnsureGetNumberMethodIsCreatedCorrectlyForTargetFields()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;
   
                       [FluentSetup]
                       public partial class PersonSetup
                       {
                           [FluentMember]
                           private int number = 123;
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WhereMethod("GetNumber")
         .IsProtected()
         .Contains("return number;");


      result.Should().HaveClass("MyTests.PersonSetup")
         .WithoutMethod("GetNumber" ,"System.Func<int>");
   }

   [TestMethod]
   public void EnsureDefaultValueWithStringEmptyIsCreatedCorrectly()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;
   
                       [FluentSetup]
                       public partial class PersonSetup
                       {
                           [FluentMember]
                           private string name = string.Empty;
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WhereMethod("GetName")
         .IsProtected()
         .Contains("return name;");
   }

   [TestMethod]
   public void EnsureDefaultValueWorksCorrectlyForSetupClassesWithDifferentNamePattern()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       public class Color
                       {
                          public string Name { get; set; }

                          public int Opacity { get; set; }
                       }

                       [FluentSetup(typeof(Color))]
                       public partial class ColorSetup
                       {
                       }

                       [FluentSetup(typeof(Color), EntryMethod = ""ColorWithDefaults"")]
                       public partial class ColorWithDefaultsSetup
                       {
                          [FluentMember]
                          private string name = string.Empty;
                       
                          [FluentMember]
                          private int opacity = -1;
                       }
}";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();
      
      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.ColorWithDefaultsSetup")
         .WhereMethod("GetName")
         .IsProtected()
         .Contains("return name;");

      result.Print();
   }
}