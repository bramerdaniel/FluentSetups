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
         .Contains("return GetNumber(() => 123);");
   }
}