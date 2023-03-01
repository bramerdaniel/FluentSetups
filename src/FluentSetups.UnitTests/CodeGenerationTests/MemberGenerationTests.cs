// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberGenerationTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests
{
   using FluentSetups.UnitTests.Setups;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class MemberGenerationTests : VerifyBase
   {
      #region Public Methods and Operators

      [TestMethod]
      public void EnsureFieldIsSetInFluentSetupMethod()
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
            .WhereMethod("WithAge")
            .Contains("age = value")
            .Contains("ageWasSet = true;")
            .Contains("return this;");
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
      public void EnsureGeneratedFieldIsSetInFluentSetupMethod()
      {
         var code = @"namespace DonnyTheDagger
                      {
                         using FluentSetups;

                         public class Person
                         {
                            public int Age { get ;set; }
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
            .HaveClass("DonnyTheDagger.PersonSetup")
            .WhereMethod("WithAge")
            .Contains("age = value")
            .Contains("ageWasSet = true;")
            .Contains("return this;");
      }

      [TestMethod]
      public void EnsureMemberCanBeInternal()
      {
         var code = @"namespace DonnyTheDagger
                      {
                         using FluentSetups;

                         [FluentSetup]
                         internal partial class PersonSetup
                         {
                         }
                      }";

         var result = Setup.SourceGeneratorTest()
            .WithSource(code)
            .Done();

         result.Should().NotHaveErrors();
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
      public void EnsurePropertyIsSetInFluentSetupMethod()
      {
         var code = @"namespace DonnyTheDagger
                      {
                         using FluentSetups;

                         [FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentMember]
                            public int Age { get ;set; }
                         }
                      }";

         var result = Setup.SourceGeneratorTest()
            .WithSource(code)
            .Done();

         result.Should().NotHaveErrors().And
            .HaveClass("DonnyTheDagger.PersonSetup")
            .WhereMethod("WithAge")
            .Contains("Age = value")
            .Contains("ageWasSet = true;")
            .Contains("return this;");
      }

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
      public async Task EnsureSetupMethodWithCustomNamePatternIsGeneratedCorrectly()
      {
         string code = @"namespace DocumentHandling
                      {
                         using FluentSetups;
                         using System.Collections.Generic;

                         [FluentSetup]
                         public partial class DocumentSetup
                         {
                            [FluentSetups.FluentMember(""Add{0}"")]
                            public IList<string> lines;
                         }
                      }";

         var result = Setup.SourceGeneratorTest()
            .WithSource(code)
            .Done();

         result.Should().NotHaveErrors();
         
         var getLinesCode = await result.Should()
            .HaveClass("DocumentHandling.DocumentSetup")
            .WhereMethod("AddLines")
            .GetCodeAsync();

         await Verify(getLinesCode)
            .UseMethodName($"{nameof(EnsureSetupMethodWithCustomNamePatternIsGeneratedCorrectly)}.AddLines");
         
         var getLineCode = await result.Should().NotHaveErrors().And
            .HaveClass("DocumentHandling.DocumentSetup")
            .WhereMethod("AddLine")
            .GetCodeAsync();

         await Verify(getLineCode)
            .UseMethodName($"{nameof(EnsureSetupMethodWithCustomNamePatternIsGeneratedCorrectly)}.AddLine");
      }

      [TestMethod]
      public void EnsureSetupMemberMethodIsGeneratedCorrectly()
      {
         var code = @"namespace DonnyTheDagger
                      {
                         using FluentSetups;

                         public class Person
                         {
                            public int Age { get ;set; }
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
            .HaveClass("DonnyTheDagger.PersonSetup")
            .WhereMethod("SetupTarget")
            .HasParameter("Person target");
      }

      [TestMethod]
      public void EnsurePropertyWithNameValueIsGeneratedCorrectly()
      {
         var code = @"namespace DonnyTheDagger
                      {
                         using FluentSetups;

                         public class Argument
                         {
                            public string Value { get ;set; }
                         }

                         [FluentSetup(typeof(Argument))]
                         public partial class ArgumentSetup
                         {
                         }
                      }";

         var result = Setup.SourceGeneratorTest()
            .WithSource(code)
            .Done();

         result.Should().NotHaveErrors().And
            .HaveClass("DonnyTheDagger.ArgumentSetup")
            .WhereMethod("WithValue")
            .Contains("this.value = value;");
      }

      #endregion
   }
}