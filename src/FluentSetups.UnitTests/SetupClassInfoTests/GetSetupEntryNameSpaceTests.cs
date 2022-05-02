// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetSetupEntryNameSpaceTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.SetupClassInfoTests
{
   using FluentAssertions;

   using FluentSetups.UnitTests.Setups;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class GetSetupEntryNameSpaceTests
   {
      [TestMethod]
      public void EnsureSetupEntryNameSpaceIsNullWhenFluentSetupsNamespaceIsMissing()
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

         setupClassInfo.GetSetupEntryNameSpace().Should().BeNull();
      }

      [TestMethod]
      public void EnsureSetupEntryNameSpaceIsNullWhenWrongNamespaceIsUsed()
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

         setupClassInfo.GetSetupEntryNameSpace().Should().BeNull();
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
            .WithName("PersonSetup")
            .WithRootNamespace("MyRootNamespace")
            .AddSource(code)
            .Done();

         setupClassInfo.GetSetupEntryNameSpace().Should().Be("MyRootNamespace");
      }

      [TestMethod]
      public void EnsureSetupEntryNameSpaceIsUsedFromClassWhenNotSpecified()
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
            .WithName("PersonSetup")
            .AddSource(code)
            .Done();

         setupClassInfo.GetSetupEntryNameSpace().Should().Be("Ronny.Balcony");
      }

      [TestMethod]
      public void EnsureSetupCustomEntryClassNameIsComputedCorrectlyWithInlineNamespace()
      {
         string code = @"namespace RonnyTheRobber
                      {
                           [FluentSetups.FluentSetup(""MyCustomSetup"")]
                           public partial class PersonSetup : ISetup<Person>
                           {
                              [FluentProperty]
                              public string Name { get; set; }
                           }
                      }";

         var setupClassInfo = Setup.SetupClassInfo()
            .WithName("PersonSetup")
            .AddSource(code)
            .Done();

         setupClassInfo.GetSetupEntryNameSpace().Should().Be("RonnyTheRobber");
      }

   }
}