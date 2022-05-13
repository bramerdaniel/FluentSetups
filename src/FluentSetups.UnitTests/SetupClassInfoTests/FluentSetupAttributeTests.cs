namespace FluentSetups.UnitTests.SetupClassInfoTests
{
   using FluentAssertions;

   using FluentSetups.UnitTests.Setups;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class FluentSetupAttributeTests
   {
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
            .WithSource(code)
            .Done();

         setupClassInfo.FluentSetupAttribute.Should().NotBeNull();

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
            .WithSource(code)
            .Done();

         setupClassInfo.FluentSetupAttribute.Should().NotBeNull();
      }

   }
}