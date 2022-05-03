// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsValidTests2.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.SetupModelTests
{
   using FluentSetups.UnitTests.Setups;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class SetupClassModelTests
   {
      #region Public Methods and Operators

      [TestMethod]
      public void EnsureClassNameIsCorrect()
      {
         string code = @"namespace RonnyTheRobber
                      {
                         [FluentSetups.FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentSetups.FluentMember]
                            public string Name { get; set; }
                         }
                      }";

         var result = Setup.SetupClassModel()
            .FromSource(code)
            .Done();

         result.Should().HaveName("PersonSetup");
      }

      [TestMethod]
      public void EnsureContainingNamespaceIsCorrect()
      {
         string code = @"namespace RonnyThePony
                      {
                         [FluentSetups.FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentSetups.FluentProperty]
                            public string Name { get; set; }
                         }
                      }";

         var result = Setup.SetupClassModel()
            .FromSource(code)
            .Done();

         result.Should().HaveContainingNamespace("RonnyThePony");
      }

      [TestMethod]
      public void EnsureContainingNamespaceIsCorrectWhenIsGlobalNamespace()
      {
         string code = @"[FluentSetups.FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentSetups.FluentProperty]
                            public string Name { get; set; }
                         }";

         var result = Setup.SetupClassModel()
            .WithRootNamespace("RonnyTheRoot")
            .FromSource(code)
            .Done();

         result.Should().HaveContainingNamespace(null);
      }

      [TestMethod]
      public void EnsurePropertiesAreComputedCorrectly()
      {
         string code = @"namespace GarryGreen;

                         [FluentSetups.FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentSetups.FluentMember]
                            public string Name { get; set; }
                         }";

         var result = Setup.SetupClassModel()
            .FromSource(code)
            .Done();

         result.Should().HaveProperty("Name")
            .WithTypeName("string").And
            .WithRequiredNamespace("System").And
            .WithSetupMethodName("WithName");
      }

      [TestMethod]
      public void EnsurePropertiesWithCustomNamesAreComputedCorrectly()
      {
         string code = @"namespace GarryGreen;

                         [FluentSetups.FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentSetups.FluentMember(""SpecifyName"")]
                            public string Name { get; set; }
                         }";

         var result = Setup.SetupClassModel()
            .FromSource(code)
            .Done();

         result.Should().HaveProperty("Name")
            .WithSetupMethodName("SpecifyName");
      }
      
      [TestMethod]
      public void EnsureWithRequiredCustomNamespaceAreComputedCorrectly()
      {
         string code = @"namespace GarryGreen;
                         using FluentSetups;
                         using System.Threading;

                         [FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentMember]
                            public Thread Name { get; set; }
                         }";

         var result = Setup.SetupClassModel()
            .FromSource(code)
            .Done();

         result.Should().HaveProperty("Name")
            .WithRequiredNamespace("System.Threading");
      }

      [TestMethod]
      public void EnsureFieldAreComputedCorrectly()
      {
         string code = @"namespace GarryGreen;

                         using FluentSetups;
                         
                         [FluentSetup]
                         public partial class PersonSetup
                         {
                            [FluentMember]
                            public int name;
                         }";

         var result = Setup.SetupClassModel()
            .FromSource(code)
            .Done();

         result.Should().HaveField("name")
            .WithTypeName("int").And
            .WithRequiredNamespace("System").And
            .WithSetupMethodName("WithName");
      }

      #endregion
   }
}