// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntryPointGenerationTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
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
   public void EnsureSetupMethodIsGeneratedCorrectly()
   {
      string code = @"namespace RonnyTheRobber
                      {
                         [FluentSetups.FluentSetup(""FSetup"")]
                         public partial class Person
                         { }
                      }";

      var result = Setup.SourceGeneratorTest()
         .AddSource(code)
         .Done();

      result.Should().HaveClass("RonnyTheRobber.FSetup")
         .WithMethod("Person");
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
         .AddSource(code)
         .Done();

      result.Should().HaveClass("RonnyTheRobber.FSetup")
         .WithMethod("Person")
         .WithMethod("Ball");
   }

   #endregion
}