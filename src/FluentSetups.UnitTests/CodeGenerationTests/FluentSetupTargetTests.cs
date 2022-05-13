// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentSetupTargetTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class FluentSetupTargetTests
{
   #region Public Methods and Operators

   [TestMethod]
   public void EnsureEmptyTargetClassIsGeneratedCorrectly()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       public class Person
                       {
                       }
   
                       [FluentSetup(TargetType = typeof(Person))]
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
   public void EnsureTargetClassWithConstructorTypeArgumentIsGeneratedCorrectly()
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
         .WithMethod("Done");
   } 
   
   [TestMethod]
   public void EnsureTargetClassWithOtherTargetNamespaceIsGeneratedCorrectly()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;
                       using Some.Namespace;
   
                       [FluentSetup(typeof(Person))]
                       public partial class PersonSetup
                       {
                       }
                   }";

      var referencedAssembly = @"namespace Some.Namespace;

                                 public class Person
                                 {
                                 }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .WithSource(referencedAssembly)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WithMethod("Done");
   }    
  
   [TestMethod]
   public void EnsureNoTargetClassWhenTargetModeIsDisabled()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       public class Person
                       {
                       }

                       [FluentSetup(typeof(Person), TargetMode=TargetMode.Disabled)]
                       public partial class PersonSetup
                       {
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WithoutMethod("Done");
   }   
   
   [TestMethod]
   public void EnsurePublicTargetPropertiesAreGeneratedCorrectly()
   {
      var code = @"namespace Root
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
         .HaveClass("Root.PersonSetup")
         .WithMethod("WithName");
   }

   [TestMethod]
   public void EnsurePrivateTargetPropertiesAreNotGeneratedCorrectly()
   {
      var code = @"using FluentSetups;

                   public class Person
                   {
                       private string Name { get; set; }
                   }

                   [FluentSetup(typeof(Person))]
                   public partial class PersonSetup
                   {
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("Root")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("PersonSetup")
         .WithoutMethod("WithName");
   }

   [TestMethod]
   public void EnsureTargetPropertiesWithPrivateSetterAreNotGeneratedCorrectly()
   {
      var code = @"using FluentSetups;

                   public class Person
                   {
                       public string Name { get; private set; }
                   }

                   [FluentSetup(typeof(Person))]
                   public partial class PersonSetup
                   {
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("Root")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("PersonSetup")
         .WithoutMethod("WithName");
   }

   [TestMethod]
   public void EnsureTargetPropertiesWithoutSetterAreNotGeneratedCorrectly()
   {
      var code = @"using FluentSetups;

                   public class Person
                   {
                       public string Name => ""Robert"";
                   }

                   [FluentSetup(typeof(Person))]
                   public partial class PersonSetup
                   {
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("Root")
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("PersonSetup")
         .WithoutMethod("WithName");
   }
   




   


   #endregion
}