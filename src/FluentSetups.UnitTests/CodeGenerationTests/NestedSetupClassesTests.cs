// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NestedSetupClassesTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class NestedSetupClassesTests
{
   #region Public Methods and Operators

   [TestMethod]
   public void EnsureNoSourceGenerationForNestedClasses()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       public class Animal
                       {
                           public string Name { get; set; }
                       }

                       public class OuterClass
                       {
                          [FluentSetup(typeof(Animal))]
                          public partial class AnimalSetup
                          {
                          }
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();
      
      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.OuterClass");

      result.Should().NotHaveClass("MyTests.OuterClass.AnimalSetup")
         .And.NotHaveClass("MyTests.AnimalSetup");

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.OuterClass+AnimalSetup")
         .WithoutMethod("Done")
         .WithoutMethod("SetupTarget")
         .WithoutMethod("WithName");
   }


   #endregion
}