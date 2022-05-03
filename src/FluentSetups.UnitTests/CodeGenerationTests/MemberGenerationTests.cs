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
         var code = @"namespace RonnyTheRobber
                      {
                         [FluentSetups.FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentSetups.FluentMember]
                            public string Name { get; set; }
                         }
                      }";

         var result = Setup.SourceGeneratorTest()
            .WithSource(code)
            .Done();

         result.Should().NotHaveErrors().And
            .HaveClass("RonnyTheRobber.PersonSetup")
            .WithMethod("WithName");
      }
     
      [TestMethod]
      public void EnsureGetterMethodsForPropertiesAreGeneratedCorrectly()
      {
         var code = @"namespace RonnyTheRobber
                      {
                         [FluentSetups.FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentSetups.FluentMember]
                            public string Name { get; set; }
                         }
                      }";

         var result = Setup.SourceGeneratorTest()
            .WithSource(code)
            .Done();

         result.Should().NotHaveErrors().And
            .HaveClass("RonnyTheRobber.PersonSetup")
            .WithMethod("GetNameOrDefault")
            .WithMethod("GetNameOrThrow");
      }

      [TestMethod]
      public void EnsureSetupMethodWithCustomNameIsGeneratedCorrectly()
      {
         string code = @"namespace RonnyTheRobber
                      {
                         [FluentSetups.FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentSetups.FluentMember(""SetName"")]
                            public string Name { get; set; }

                         }
                      }";

         var result = Setup.SourceGeneratorTest()
            .WithSource(code)
            .Done();

         result.Should().NotHaveErrors().And
            .HaveClass("RonnyTheRobber.PersonSetup")
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
                            [FluentMember]
                            public string Name { get; set; }

                         }
                      }";

         var result = Setup.SourceGeneratorTest()
            .WithSource(code)
            .Done();

         result.FailWith("CS0246", "Type FluentProperty not found");
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
            .WithSource(code)
            .Done();

         result.Should().NotHaveErrors().And
            .HaveClass("RonnyTheRobber.PersonSetup")
            .WithoutMethod("WithName");
      }

      [TestMethod]
      public void EnsureFieldsAreGeneratedCorrectly()
      {
         var code = @"namespace DonnyTheDagger
                      {
                         using FluentSetups;

                         [FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentMember]
                            public int age;
                         }
                      }";

         var result = Setup.SourceGeneratorTest()
            .WithSource(code)
            .Done();

         result.Should().NotHaveErrors().And
            .HaveClass("DonnyTheDagger.PersonSetup")
            .WithMethod("WithAge");
      }
      
      [TestMethod]
      public void EnsureGetMethodsIsGeneratedCorrectly()
      {
         var code = @"namespace DonnyTheDagger
                      {
                         using FluentSetups;

                         [FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentMember]
                            public int age;
                         }
                      }";

         var result = Setup.SourceGeneratorTest()
            .WithSource(code)
            .Done();

         result.Should().NotHaveErrors().And
            .HaveClass("DonnyTheDagger.PersonSetup")
            .WithMethod("GetAgeOrDefault")
            .WithMethod("GetAgeOrThrow");
      }

      #endregion
   }
}