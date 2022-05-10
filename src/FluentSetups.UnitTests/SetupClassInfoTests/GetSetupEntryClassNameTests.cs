// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetSetupEntryClassNameTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.SetupClassInfoTests;

using System;

using FluentAssertions;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class GetSetupEntryClassNameTests
{
   #region Public Methods and Operators

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
         .WithSource(code)
         .Done();

      setupClassInfo.GetSetupEntryClassName().Should().Be("Setup");
   }

   [TestMethod]
   public void EnsureSetupCustomEntryClassNameIsComputedCorrectlyWithArguments()
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
         .WithSource(code)
         .Done();

      setupClassInfo.GetSetupEntryClassName().Should().Be("MyCustomSetup");
   }

   [TestMethod]
   public void EnsureSetupCustomEntryClassNameIsComputedCorrectlyWithInlineNamespace()
   {
      string code = @"
                        [FluentSetups.FluentSetup(""MyCustomSetup"")]
                        public partial class PersonSetup : ISetup<Person>
                        {
                           [FluentProperty]
                           public string Name { get; set; }

                        }";

      var setupClassInfo = Setup.SetupClassInfo()
         .WithSource(code)
         .Done();

      setupClassInfo.GetSetupEntryClassName().Should().Be("MyCustomSetup");
   }

   [TestMethod]
   public void EnsureSetupEntryClassNameIsNullWhenFluentSetupsNamespaceIsMissing()
   {
      string code = @"[FluentSetup]
                      public partial class PersonSetup
                      {
                         [FluentProperty]
                         public string Name { get; set; }

                      }";

      Setup.SetupClassInfo()
         .WithSource(code)
         .Invoking(x => x.Done())
         .Should().Throw<ArgumentException>();
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

      Setup.SetupClassInfo()
         .WithSource(code)
         .Invoking(x => x.Done())
         .Should().Throw<ArgumentException>();
   }

   #endregion
}