namespace FluentSetups.UnitTests.SetupClassInfoTests
{
   using FluentAssertions;

   using FluentSetups.UnitTests.Setups;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class SetupClassInfoTests2
   {
      [TestMethod]
      public void EnsureSetupClassInfoIsComputedCorrectly()
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

         setupClassInfo.ClassName.Should().Be("PersonSetup");
         setupClassInfo.FluentSetupAttribute.Should().NotBeNull();
         setupClassInfo.ClassSymbol.Should().NotBeNull();
         setupClassInfo.GetSetupEntryClassName().Should().Be("Setup");
      }

      [TestMethod]
      public void EnsureSetupDefaultEntryClassNameIsComputedCorrectly()
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
}