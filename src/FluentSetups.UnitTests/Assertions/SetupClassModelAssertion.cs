// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupClassModelAssertion.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.Assertions;

using System.Linq;

using FluentAssertions;
using FluentAssertions.Primitives;

using FluentSetups.SourceGenerator.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

internal class SetupClassModelAssertion : ReferenceTypeAssertions<SetupClassModel, SetupClassModelAssertion>
{
   #region Constructors and Destructors

   public SetupClassModelAssertion(SetupClassModel target)
      : base(target)
   {
   }

   #endregion

   #region Properties

   protected override string Identifier => nameof(SetupClassModelAssertion);

   #endregion

   #region Public Methods and Operators

   public AndConstraint<SetupClassModelAssertion> HaveContainingNamespace(string expectedNamespace)
   {
      Assert.AreEqual(expectedNamespace, Subject.ContainingNamespace);
      return new AndConstraint<SetupClassModelAssertion>(this);
   }

   public AndConstraint<SetupClassModelAssertion> HaveName(string expectedName)
   {
      Assert.AreEqual(expectedName, Subject.ClassName);
      return new AndConstraint<SetupClassModelAssertion>(this);
   }

   public SetupPropertyModelAssertion HaveProperty(string expectedPropertyName)
   {
      var property = Subject.Properties.FirstOrDefault(p => p.PropertyName == expectedPropertyName);
      Assert.IsNotNull(property);

      return new SetupPropertyModelAssertion(property);
   }

   #endregion

   internal class SetupPropertyModelAssertion : ReferenceTypeAssertions<SetupPropertyModel, SetupPropertyModelAssertion>
   {
      #region Constructors and Destructors

      public SetupPropertyModelAssertion(SetupPropertyModel subject)
         : base(subject)
      {
      }

      #endregion

      #region Properties

      protected override string Identifier => nameof(SetupPropertyModelAssertion);

      #endregion

      #region Public Methods and Operators

      public AndConstraint<SetupPropertyModelAssertion> WithSetupMethodName(string expectedName)
      {
         Assert.AreEqual(expectedName, Subject.SetupMethodName);
         return new AndConstraint<SetupPropertyModelAssertion>(this);
      }

      #endregion

      public AndConstraint<SetupPropertyModelAssertion> WithTypeName(string expectedTypeName)
      {
         Assert.AreEqual(expectedTypeName, Subject.TypeName);
         return new AndConstraint<SetupPropertyModelAssertion>(this);
      }

      public AndConstraint<SetupPropertyModelAssertion> WithRequiredNamespace(string expectedNamespace)
      {
         Assert.AreEqual(expectedNamespace, Subject.RequiredNamespace);
         return new AndConstraint<SetupPropertyModelAssertion>(this);
      }

   }
}