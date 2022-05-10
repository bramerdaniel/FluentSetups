// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenerationResultAssertion.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;

   using FluentAssertions;
   using FluentAssertions.Primitives;

   using FluentSetups.UnitTests.Setups;

   using Microsoft.CodeAnalysis;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   internal class GenerationResultAssertion : ReferenceTypeAssertions<GenerationResult, GenerationResultAssertion>
   {
      private string expectedError;

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
         var classType = Subject.Compilation.GetTypeByMetadataName(className);
         
         Assert.IsNotNull(classType, $"The class {className} could not be found. {Environment.NewLine}{Subject.SyntaxTrees.Last().ToString()}");
         return new ClassAssertion(Subject, classType);
      }

      public ClassAssertion HavePartialClass(string className)
      {
         var classAssertion = HaveClass(className);
         classAssertion.MustBePartial();
         return classAssertion;
      }

      #endregion

      public AndConstraint<GenerationResultAssertion> NotHaveErrors()
      {
         ThrowOnErrors(Subject.GeneratedDiagnostics);
         ThrowOnErrors(Subject.Compilation.GetDiagnostics());

         return new AndConstraint<GenerationResultAssertion>(this);
      }

      private void ThrowOnErrors(IEnumerable<Diagnostic> diagnostics)
      {
         var errorDiagnostic = diagnostics.FirstOrDefault(x => x.Severity == DiagnosticSeverity.Error);
         if (errorDiagnostic != null)
            throw new AssertFailedException(CreateMessage(errorDiagnostic));
      }

      private static string CreateMessage(Diagnostic errorDiagnostic)
      {
         var builder = new StringBuilder();
         builder.AppendLine("MESSAGE");
         builder.AppendLine(errorDiagnostic.GetMessage());
         builder.AppendLine("SOURCE");
         builder.AppendLine(errorDiagnostic.Location?.SourceTree?.ToString());
         return builder.ToString();
      }
   }
}