// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListGenerationTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.CodeGenerationTests;

using FluentSetups.UnitTests.Setups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ListGenerationTests : VerifyBase
{
   #region Public Methods and Operators

   [TestMethod]
   [Ignore]
   public async Task EnsureListSetupForTargetEnumerableCorrectly()
   {
      var code = @"namespace MyTests
                   {
                       using FluentSetups;
                       using System.Collections.Generic;

                       public class Bag
                       {
                          public IEnumerable<string> Values { get; set; }
                       }
   
                       [FluentSetup(typeof(Bag))]
                       public partial class BagSetup
                       {
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();
      result.Should().NotHaveErrors();

      var withElements = await result.Should().NotHaveErrors().And
         .HaveClass("MyTests.BagSetup")
         .WhereMethod("WithElements")
         .GetCodeAsync();

      await Verify(withElements).UseMethodName("EnumerableTargetProperty.WithElements");

      var methodCode = await result.Should().HaveClass("MyTests.BagSetup")
         .WhereMethod("WithElement")
         .GetCodeAsync();

      await Verify(methodCode)
         .UseFileName("EnumerableTargetProperty.WithElement");

      result.Print();
   }

   [TestMethod]
   public void EnsureFullListSetupIsGeneratedCorrectly()
   {
      var code = @"namespace MyTests
                   {
                       using System.Collections.Generic;
                       using FluentSetups;

                       public class Child
                       {
                           public string Name{ get; set; }
                       }

                       public class School
                       {
                           public IList<Child> Children{ get; set; }
                       }
   
                       [FluentSetup(typeof(School))]
                       public partial class SchoolSetup
                       {  
                          [FluentMember]
                          private IList<Child> children;
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.SchoolSetup")
         .WhereMethod("WithChildren")
         .IsPublic();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.SchoolSetup")
         .WithMethod("WithChildren")
         .WhereMethod("WithChild")
         .IsPublic()
         .Contains("children.Add(value);");

      result.Print();
   }

   [TestMethod]
   public async Task EnsureInternalSetupForListIsGeneratedCorrectly()
   {
      var code = @"namespace MyTests
                   {
                       using System.Collections.Generic;
                       using FluentSetups;

                       public class Bag
                       {
                           public IList<int> Elements{ get; set; }
                       }
   
                       [FluentSetup(typeof(Bag))]
                       internal partial class BagSetup
                       {  
                          [FluentMember]
                          private IList<int> elements;
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.BagSetup")
         .WhereMethod("WithElements")
         .IsInternal();

      result.Should().HaveClass("MyTests.BagSetup")
         .WhereMethod("WithElement")
         .IsInternal()
         .Contains("if (elements == null)")
         .Contains("elements = new List<int>()")
         .Contains("elements.Add(value);");

      var methodCode = await result.Should().HaveClass("MyTests.BagSetup")
         .WhereMethod("WithElement")
         .GetCodeAsync();

      await Verify(methodCode)
         .UseMethodName("WithElement");

      result.Print();
   }

   [TestMethod]
   public void EnsureSetupForListIsGeneratedCorrectly()
   {
      var code = @"namespace MyTests
                   {
                       using System.Collections.Generic;
                       using FluentSetups;

                       public class Bag
                       {
                           public IList<int> Elements{ get; set; }
                       }
   
                       [FluentSetup(typeof(Bag))]
                       public partial class BagSetup
                       {  
                          [FluentMember]
                          private IList<int> elements;
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();


      result.Should().NotHaveErrors().And
         .HaveClass("MyTests.BagSetup")
         .WhereMethod("WithElements")
         .IsPublic();

      result.Should().HaveClass("MyTests.BagSetup")
         .WhereMethod("WithElement")
         .IsPublic()
         .Contains("if (elements == null)")
         .Contains("elements = new List<int>()")
         .Contains("elements.Add(value);");

      result.Print();

   }

   [TestMethod]
   public async Task EnsureListSetupForEnumerableIsGeneratedCorrectly()
   {
      var code = @"namespace MyTests
                   {
                       using System.Collections.Generic;
                       using FluentSetups;

                       public class Bag
                       {
                           public IEnumerable<int> Elements{ get; set; }
                       }
   
                       [FluentSetup(typeof(Bag))]
                       internal partial class BagSetup
                       {  
                          [FluentMember]
                          private IList<int> elements;
                       }
                   }";

      var result = Setup.SourceGeneratorTest()
         .WithSource(code)
         .Done();

      result.Should().NotHaveErrors();

      var withElements = await result.Should().NotHaveErrors().And
         .HaveClass("MyTests.BagSetup")
         .WhereMethod("WithElements")
         .GetCodeAsync();

      await Verify(withElements).UseMethodName("EnumerableField.WithElements");

      var methodCode = await result.Should().HaveClass("MyTests.BagSetup")
         .WhereMethod("WithElement")
         .GetCodeAsync();

      await Verify(methodCode)
         .UseFileName("EnumerableField.WithElement");

      result.Print();
   }

   #endregion
}