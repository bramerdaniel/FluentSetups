// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntryPointGenerationTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
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
   public void EnsureExistingEntryClassWithDifferentModifierAdjustGeneratedModifier()
   {
      string code = @"namespace MyUnitTests
                      {  
                         using FluentSetups;

                         public partial class Setup { }

                         [FluentSetup]
                         public partial class PersonSetup { }
                      }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("MyUnitTests")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyUnitTests.Setup")
         .WithPublicModifier()
         .WithStaticMethod("Person");

      result.Should()
         .HaveClass("MyUnitTests.Setup")
         .WithoutCode("NOT SUPPORTED");
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
         .WithRootNamespace("RonnyTheRobber")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RonnyTheRobber.FSetup")
         .WithStaticMethod("Person")
         .WithStaticMethod("Ball");
   }

   [TestMethod]
   public void EnsureSetupEntryMethodIsGeneratedCorrectly()
   {
      string code = @"namespace MyUnitTests
                      {  
                         using FluentSetups;

                         [FluentSetup]
                         public partial class PersonSetup { }
                      }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("MyUnitTests")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyUnitTests.Setup")
         .WithInternalModifier()
         .WithStaticMethod("Person");
   }

   [TestMethod]
   public void EnsureSetupInRootNamespaceIsGeneratedCorrectly()
   {
      string code = @"
                      public class Person{ }
                      
                      [FluentSetups.FluentSetup(typeof(Person))]
                      internal partial class PersonSetup { }
                      ";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("MyRoot")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyRoot.Setup")
         .WithInternalModifier()
         .WithStaticMethod("Person");
   }

   [TestMethod]
   public void EnsureSetupMethodIsGeneratedCorrectly()
   {
      string code = @"namespace RonnyTheRobber
                      {
                         [FluentSetups.FluentSetup(""FSetup"")]
                         public partial class Person { }
                      }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("MyRoot")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyRoot.FSetup")
         .WithInternalModifier()
         .WithStaticMethod("Person");
   }

   [TestMethod]
   public void EnsureSetupTypesFromDifferentNamespacesWorkCorrectly()
   {
      string target = @"namespace FirstNamespace
                      {
                         [FluentSetups.FluentSetup]
                         public partial class FirstSetup
                         {
                         }
                      }";

      string code = @"namespace SecondNamespace
                      {
                         [FluentSetups.FluentSetup]
                         public partial class SecondSetup
                         {
                         }
                      }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("RootNamespace")
         .WithSource(target)
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RootNamespace.Setup")
         .WithStaticMethod("First")
         .WithStaticMethod("Second");
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
                         using FluentSetups;
                         using SomeOther.ModelNameSpace;

                         [FluentSetup]
                         public partial class PersonSetup
                         {
                            public Person Done() => new Person();
                         }
                      }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("DefaultAssemblyNamespace")
         .WithSource(target)
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("DefaultAssemblyNamespace.Setup")
         .WithStaticMethod("Person");
   }

   [TestMethod]
   public void EnsureDefaultNamingOfTargetIsUsed()
   {
      var code = @"namespace SetupNameSpace
                   {
                      using FluentSetups;

                      [FluentSetup(typeof(Person))]
                      public partial class SomeCustomName
                      {
                      }

                      public class Person
                      { 
                      }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RootNamespace.Setup")
         .WithStaticMethod("Person");
   }

   [TestMethod]
   public void EnsureDifferentEntryPointMethodsArePossible()
   {
      var code = @"namespace SetupNameSpace
                   {
                      using FluentSetups;

                      [FluentSetup(typeof(Person))]
                      public partial class First
                      {
                      }

                      [FluentSetup(typeof(Person), SetupMethod = ""Custom"")]
                      public partial class Second
                      {
                      }

                      public class Person
                      { 
                      }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RootNamespace.Setup")
         .WithStaticMethod("Person")
         .WithStaticMethod("Custom");
   }

   [TestMethod]
   public void EnsureNoWarningsForMultipleEntryMethods()
   {
      var code = @"namespace SetupNameSpace
                   {
                      using FluentSetups;

                      [FluentSetup(typeof(Person))]
                      public partial class PersonSetup
                      {
                      }

                      [FluentSetup(typeof(Car))]
                      public partial class CarSetup
                      {
                      }

                      public class Person
                      { 
                      }

                      public class Car
                      { 
                      }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveWarnings().And
         .HaveClass("RootNamespace.Setup")
         .WithStaticMethod("Person")
         .WithStaticMethod("Car");
   }
  
   [TestMethod]
   public void EnsureFluentRootCanBeSpecified()
   {
      var code = @"namespace SetupNameSpace
                   {
                      using FluentSetups;

                      [FluentRoot]
                      public partial class Create
                      {
                      }

                      [FluentSetup(typeof(Person))]
                      public partial class PersonSetup
                      {
                      }

                      public class Person { }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveWarnings().And
         .HaveClass("SetupNameSpace.Create")
         .WithStaticMethod("Person");
      
      result.Should()
         .NotHaveClass("RootNamespace.Create");
   }  
   
   [TestMethod]
   public void EnsureFluentRootBeSpecifiedInADifferentNamespace()
   {
      var code = @"namespace SetupNameSpace
                   {
                      using FluentSetups;

                      [FluentSetup(typeof(Person))]
                      public partial class PersonSetup
                      {
                      }

                      public class Person { }
                   }";
      
      var root = @"namespace HereGoesTheRoot
                   {
                      using FluentSetups;

                      [FluentRoot]
                      public partial class Create
                      {
                      }
                   }";
      
      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .WithSource(root)
         .Done();

      result.Should().NotHaveWarnings().And
         .HaveClass("HereGoesTheRoot.Create")
         .WithStaticMethod("Person");
      
      result.Should()
         .NotHaveClass("RootNamespace.Create");
      
      result.Print();
   }

   #endregion
}