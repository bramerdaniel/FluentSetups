// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetModeTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.SetupModelTests;

using FluentSetups.SourceGenerator.Models;
using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class TargetModeTests
{
   #region Public Methods and Operators
   
   [TestMethod]
   public void EnsureTargetModeDisabledIsComputedCorrectly()
   {
      string code = @"namespace RonnyTheRobber
                      {
                         using FluentSetups;

                         [FluentSetup(typeof(Person), TargetMode=TargetMode.Disabled)]
                         public partial class PersonSetup
                         {
                         }

                         public class Person
                         {
                         }
                      }";

      var result = Setup.SetupClassModel()
         .FromSource(code)
         .Done();

      result.Should().HaveTargetMode(TargetGenerationMode.Disabled);
   }

   [TestMethod]
   public void EnsureTargetModeAutoIsComputedCorrectly()
   {
      string code = @"namespace RonnyTheRobber
                      {
                         using FluentSetups;

                         [FluentSetup(typeof(Person), TargetMode=TargetMode.Automatic)]
                         public partial class PersonSetup
                         {
                         }

                         public class Person
                         {
                         }
                      }";

      var result = Setup.SetupClassModel()
         .FromSource(code)
         .Done();

      result.Should().HaveTargetMode(TargetGenerationMode.Automatic);
   }


   [TestMethod]
   public void EnsureDefaultTargetModeAutoIsComputedCorrectly()
   {
      string code = @"namespace RonnyTheRobber
                      {
                         using FluentSetups;

                         [FluentSetup(typeof(Person))]
                         public partial class PersonSetup
                         {
                         }

                         public class Person
                         {
                         }
                      }";

      var result = Setup.SetupClassModel()
         .FromSource(code)
         .Done();
      
      result.Should().HaveTargetMode(TargetGenerationMode.Automatic);
   }

   #endregion
}