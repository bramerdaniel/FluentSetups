// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsValidTests2.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests
{
   using FluentSetups.UnitTests.Setups;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class MemberGenerationTests
   {
      #region Public Methods and Operators

      [TestMethod]
      public void EnsureSetupMethodIsGeneratedCorrectly()
      {
         string code = @"namespace RonnyTheRobber
                      {
                         [FluentSetups.FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentSetups.FluentProperty]
                            public string Name { get; set; }
                         }
                      }";

         var result = Setup.SourceGeneratorTest()
            .AddSource(code)
            .Done();

         result.Should().HaveClass("RonnyTheRobber.PersonSetup")
            .WithMethod("WithName");
      }

      [TestMethod]
      public void EnsureSetupMethodWithCustomNameIsGeneratedCorrectly()
      {
         string code = @"namespace RonnyTheRobber
                      {
                         [FluentSetups.FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentSetups.FluentProperty(""SetName"")]
                            public string Name { get; set; }

                         }
                      }";

         var result = Setup.SourceGeneratorTest()
            .AddSource(code)
            .Done();

         result.Should().HaveClass("RonnyTheRobber.PersonSetup")
            .WithMethod("SetName");
      }

      [TestMethod]
      public void EnsureNoSetupMethodWithMissingAttributeNamespace()
      {
         string code = @"namespace RonnyTheRobber
                      {
                         [FluentSetups.FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentProperty]
                            public string Name { get; set; }

                         }
                      }";

         var result = Setup.SourceGeneratorTest()
            .AddSource(code)
            .Done();

         result.Should().HaveClass("RonnyTheRobber.PersonSetup")
            .WithoutMethod("WithName");
      }
      
      [TestMethod]
      public void EnsureNoSetupMethodsForPropertiesWithoutAttribute()
      {
         string code = @"namespace RonnyTheRobber
                      {
                         [FluentSetups.FluentSetup]
                         public partial class PersonSetup
                         {
                            public string Name { get; set; }
                         }
                      }";

         var result = Setup.SourceGeneratorTest()
            .AddSource(code)
            .Done();

         result.Should().HaveClass("RonnyTheRobber.PersonSetup")
            .WithoutMethod("WithName");
      }

      #endregion
   }
}