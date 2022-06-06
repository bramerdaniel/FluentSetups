// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Class1.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.AnalyzerTests;

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using FluentSetups.SourceGenerator.Analyzers;

using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
[SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions")]
public class MultiplePartialSetupMembersAnalyzerTests : FluentSetupAnalyzerTest<MultiplePartialSetupMembersAnalyzer>
{
   #region Public Methods and Operators

   [TestMethod]
   public async Task EnsureCorrectResultForIgnoredFluentSetupClasses()
   {
      string code = @"
                         using FluentSetups;

                         [{|#0:FluentSetup|}]
                         public partial class PersonSetup
                         {
                         }

                         public partial class PersonSetup
                         {
                         }
                      ";

      var descriptor = new MultiplePartialSetupMembersAnalyzer().SupportedDiagnostics.First();

      ExpectDiagnostic(descriptor, d => d.WithLocation(0).WithArguments("PersonSetup").WithSeverity(DiagnosticSeverity.Info));
      await RunAsync(code);
   }


   [TestMethod]
   public async Task EnsureNoResultsForNotFluentSetupClasses()
   {
      string code = @"
                         using FluentSetups;

                         public partial class PersonSetup
                         {
                         }

                         public partial class PersonSetup
                         {
                         }
                      ";

      await RunAsync(code);
   }

   [TestMethod]
   public async Task EnsureNoResultsForGeneratedFluentSetupClasses()
   {
      string code = @"
                         using FluentSetups;

                         [FluentSetup]
                         public partial class PersonSetup
                         {
                         }
                      ";

      await RunAsync(code);
   }


   [TestMethod]
   public async Task EnsureNoResultsForEmptyCode()
   {
      await RunAsync("");
   }


   #endregion
}