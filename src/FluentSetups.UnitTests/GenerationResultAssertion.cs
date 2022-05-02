// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenerationResultAssertion.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests
{
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
         var classType = Subject.Compilation.GetTypeByMetadataName(className);
         
         Assert.IsNotNull(classType, $"The class {className} could not be found");
         return new ClassAssertion(classType);
      }

      public ClassAssertion HavePartialClass(string className)
      {
         var classAssertion = HaveClass(className);
         classAssertion.MustBePartial();
         return classAssertion;
      }

      #endregion
   }
}