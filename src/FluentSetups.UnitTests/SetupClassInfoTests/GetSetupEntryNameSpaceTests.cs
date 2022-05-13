// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetSetupEntryNameSpaceTests.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.SetupClassInfoTests
{
   using System;

   using FluentAssertions;

   using FluentSetups.UnitTests.Setups;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class GetSetupEntryNameSpaceTests
   {
      #region Public Methods and Operators

      [TestMethod]
      public void EnsureRootNamespaceIsUsedWhenNotSpecifiedExplicitly()
      {
         string code = @"namespace Ronny.Balcony
                      {
                           using FluentSetups;                        

                           [FluentSetup]
                           public partial class PersonSetup : ISetup<Person>
                           {
                              [FluentProperty]
                              public string Name { get; set; }

                           }
                      }";

         var setupClassInfo = Setup.SetupClassInfo()
            .WithRootNamespace("My.Name.Space")
            .WithSource(code)
            .Done();

         setupClassInfo.GetSetupEntryNameSpace().Should().Be("My.Name.Space");
      }

      [TestMethod]
      public void EnsureSetupEntryNameSpaceIsNullWhenFluentSetupsNamespaceIsMissing()
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
      public void EnsureSetupEntryNameSpaceIsNullWhenWrongNamespaceIsUsed()
      {
         string code = @"[AnyOtherName.FluentSetup]
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
      public void EnsureSetupEntryNameSpaceIsRootNamespaceWhenNoNamespaceIsDefined()
      {
         string code = @"using FluentSetups;   

                      [FluentSetup]
                      public partial class PersonSetup : ISetup<Person>
                      {
                         [FluentProperty]
                         public string Name { get; set; }

                      }";

         var setupClassInfo = Setup.SetupClassInfo()
            .WithRootNamespace("MyRootNamespace")
            .WithSource(code)
            .Done();

         setupClassInfo.GetSetupEntryNameSpace().Should().Be("MyRootNamespace");
      }

      #endregion
   }
}