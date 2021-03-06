// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartialClassHandlingTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests;

using FluentSetups.UnitTests.Setups;

[TestClass]
public class PartialClassHandlingTests
{
   #region Public Methods and Operators

   [TestMethod]
   public void EnsureNoSourceGenerationWhenPartialClassHasMultipleLocalParts()
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

                       public partial class PersonSetup
                       {
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      Assert.AreEqual(1, result.InputSyntaxTrees.Count);
      Assert.AreEqual(2, result.OutputSyntaxTrees.Count);

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.PersonSetup")
         .WithoutMethod("Done")
         .WithoutMethod("SetupTarget")
         .WithoutMethod("WithName");

      result.Should().NotHaveErrors();
      //// result.Should().HaveDiagnostic("FSI0001");
   }

   [TestMethod]
   public void EnsureSetupMethodForMultipleLocalParts()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;

                       public class Person
                       {
                           public string Name { get; set; }
                       }
   
                       [FluentSetup(typeof(Person))]
                       internal partial class PersonSetup
                       {
                       }

                       internal partial class PersonSetup
                       {
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("RootNamespace.Setup")
         .WithInternalMethod("Person");
   }

   #endregion
}