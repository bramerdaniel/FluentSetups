// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetSetupEntryClassNameTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.SetupClassInfoTests;

using FluentAssertions;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class GetSetupEntryClassNameTests
{
   [TestMethod]
   public void EnsureSetupEntryClassNameIsNullWhenFluentSetupsNamespaceIsMissing()
   {
      string code = @"[FluentSetup]
                      public partial class PersonSetup : ISetup<Person>
                      {
                         [FluentProperty]
                         public string Name { get; set; }

                      }";

      var setupClassInfo = Setup.SetupClassInfo()
         .WithName("PersonSetup")
         .AddSource(code)
         .Done();

      setupClassInfo.GetSetupEntryClassName().Should().BeNull();
   }

   [TestMethod]
   public void EnsureSetupEntryClassNameIsNullWhenWrongNamespaceIsUsed()
   {
      string code = @"[AnyOtherName.FluentSetup]
                      public partial class PersonSetup : ISetup<Person>
                      {
                         [FluentProperty]
                         public string Name { get; set; }

                      }";

      var setupClassInfo = Setup.SetupClassInfo()
         .WithName("PersonSetup")
         .AddSource(code)
         .Done();

      setupClassInfo.GetSetupEntryClassName().Should().BeNull();
   }

   [TestMethod]
   public void EnsureEntryClassNameIsComputedCorrectlyWithCorrectNamespace()
   {
      string code = @"using FluentSetups;   

                      [FluentSetup]
                      public partial class PersonSetup : ISetup<Person>
                      {
                         [FluentProperty]
                         public string Name { get; set; }

                      }";

      var setupClassInfo = Setup.SetupClassInfo()
         .WithName("PersonSetup")
         .AddSource(code)
         .Done();

      setupClassInfo.GetSetupEntryClassName().Should().Be("Setup");
   }

   [TestMethod]
   public void EnsureSetupCustomEntryClassNameIsComputedCorrectly()
   {
      string code = @"
                        using FluentSetups;                        

                        [FluentSetup(""MyCustomSetup"")]
                        public partial class PersonSetup : ISetup<Person>
                        {
                           [FluentProperty]
                           public string Name { get; set; }

                        }";

      var setupClassInfo = Setup.SetupClassInfo()
         .WithName("PersonSetup")
         .AddSource(code)
         .Done();

      setupClassInfo.GetSetupEntryClassName().Should().Be("MyCustomSetup");
   }

   [TestMethod]
   public void EnsureSetupCustomEntryClassNameIsComputedCorrectly2()
   {
      string code = @"
                        [FluentSetups.FluentSetup(""MyCustomSetup"")]
                        public partial class PersonSetup : ISetup<Person>
                        {
                           [FluentProperty]
                           public string Name { get; set; }

                        }";

      var setupClassInfo = Setup.SetupClassInfo()
         .WithName("PersonSetup")
         .AddSource(code)
         .Done();

      setupClassInfo.GetSetupEntryClassName().Should().Be("MyCustomSetup");
   }

}