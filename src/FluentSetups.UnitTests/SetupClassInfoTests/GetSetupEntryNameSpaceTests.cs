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
            .WithSource(code)
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
            .WithSource(code)
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
            .WithSource(code)
            .Done();

         setupClassInfo.GetSetupEntryNameSpace().Should().Be("MyRootNamespace");
      }

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
            .WithName("PersonSetup")
            .WithSource(code)
            .Done();

         setupClassInfo.GetSetupEntryNameSpace().Should().Be("My.Name.Space");
      }
   }
}