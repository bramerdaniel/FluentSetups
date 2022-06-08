// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HidingEntryMethodTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests.HidingMembers;

using FluentSetups.UnitTests.Setups;

[TestClass]
public class HidingEntryMethodTests
{
   #region Public Methods and Operators

   [TestMethod]
   public void EnsureNoEntryMethodIsGeneratedWhenDefinedByUser()
   {
      var code = @"using FluentSetups;

                   public class Person
                   {
                       public string Name { get; set; }
                   }

                   [FluentSetup(typeof(Person))]
                   internal partial class PersonSetup
                   {
                   }
                  ";

      var setup = @"namespace Root;
                   
                   internal partial class Setup
                   {
                      internal PersonSetup Person() => new PersonSetup(); 
                   }
                  ";

      var result = Setup.SourceGeneratorTest()
         .WithRootNamespace("Root")
         .WithSource(code)
         .WithSource(setup)
         .Done();

      result.Should().NotHaveErrors();

      result.Should().NotHaveErrors().And
         .HaveClass("Root.Setup")
         .WithInternalMethod("Person");

      result.Print();
   }

   #endregion
}