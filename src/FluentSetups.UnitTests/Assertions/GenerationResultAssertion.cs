// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenerationResultAssertion.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.Assertions
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Linq;
   using System.Text;

   using FluentAssertions;
   using FluentAssertions.Primitives;

   using FluentSetups.UnitTests.Setups;

   using Microsoft.CodeAnalysis;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   internal class GenerationResultAssertion : ReferenceTypeAssertions<GenerationResult, GenerationResultAssertion>
   {
      #region Constructors and Destructors

      public GenerationResultAssertion(GenerationResult subject)
         : base(subject)
      {
      }

      #endregion

      #region Properties

      protected override string Identifier => nameof(GenerationResultAssertion);

      #endregion

      #region Public Methods and Operators

      public ClassAssertion HaveClass(string className)
      {
         var classType = Subject.OutputCompilation.GetTypeByMetadataName(className);

         Assert.IsNotNull(classType, $"The class {className} could not be found. {Environment.NewLine}{Subject.OutputSyntaxTrees.Last().ToString()}");
         return new ClassAssertion(Subject, classType);
      }

      public AndConstraint<GenerationResultAssertion> NotHaveClass(string className)
      {
         var classType = Subject.OutputCompilation.GetTypeByMetadataName(className);

         Assert.IsNull(classType, $"The class {className} was found but it should not exist. {Environment.NewLine}{Subject.OutputSyntaxTrees.Last()}");
         return new AndConstraint<GenerationResultAssertion>(this);
      }

      public AndConstraint<GenerationResultAssertion> HaveDiagnostic(string diagnosticId)
      {
         var diagnostic = Subject.GeneratedDiagnostics.FirstOrDefault(d => d.Id == diagnosticId);
         Assert.IsNotNull(diagnostic, $"Diagnostic {diagnostic} could not be found.{CreateAllFound(Subject.GeneratedDiagnostics)}");

         return new AndConstraint<GenerationResultAssertion>(this);

         string CreateAllFound(ImmutableArray<Diagnostic> generatedDiagnostics)
         {
            return $"{Environment.NewLine}{string.Join(Environment.NewLine, generatedDiagnostics)}";
         }
      }



      public AndConstraint<GenerationResultAssertion> NotHaveErrors()
      {
         ThrowOnErrors(Subject.GeneratedDiagnostics);
         ThrowOnErrors(Subject.OutputCompilation.GetDiagnostics());

         return new AndConstraint<GenerationResultAssertion>(this);
      }

      public AndConstraint<GenerationResultAssertion> NotHaveWarnings()
      {
         ThrowOnWarnings(Subject.GeneratedDiagnostics);
         ThrowOnWarnings(Subject.OutputCompilation.GetDiagnostics());

         return new AndConstraint<GenerationResultAssertion>(this);
      }

      #endregion

      #region Methods

      private static string CreateMessage(Diagnostic errorDiagnostic)
      {
         var builder = new StringBuilder();
         builder.AppendLine("MESSAGE");
         builder.AppendLine($"{errorDiagnostic.Id}: {errorDiagnostic.GetMessage()}");     
         builder.AppendLine("SOURCE");
         builder.AppendLine(errorDiagnostic.Location.SourceTree?.ToString());
         return builder.ToString();
      }

      private void ThrowOnErrors(IEnumerable<Diagnostic> diagnostics)
      {
         var errorDiagnostic = diagnostics.FirstOrDefault(x => x.Severity == DiagnosticSeverity.Error);
         if (errorDiagnostic != null)
            throw new AssertFailedException(CreateMessage(errorDiagnostic));
      }

      private void ThrowOnWarnings(IEnumerable<Diagnostic> diagnostics)
      {
         var errorDiagnostic = diagnostics.FirstOrDefault(x => x.Severity >= DiagnosticSeverity.Warning);
         if (errorDiagnostic != null)
            throw new AssertFailedException(CreateMessage(errorDiagnostic));
      }

      #endregion
   }
}