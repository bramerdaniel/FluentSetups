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

   public AndConstraint<SetupClassModelAssertion> HaveTargetTypeName(string expectedTypeName)
   {
      Assert.AreEqual(expectedTypeName, Subject.TargetTypeName);
      return new AndConstraint<SetupClassModelAssertion>(this);
   }

   public AndConstraint<SetupClassModelAssertion> HaveTargetTypeNamespace(string expectedNamespace)
   {
      Assert.AreEqual(expectedNamespace, Subject.TargetTypeNamespace);
      return new AndConstraint<SetupClassModelAssertion>(this);
   }

   public SetupMemberModelAssertion HaveProperty(string expectedPropertyName)
   {
      var property = Subject.Properties.FirstOrDefault(p => p.MemberName == expectedPropertyName);
      Assert.IsNotNull(property);

      return new SetupMemberModelAssertion(property);
   }

   public SetupMemberModelAssertion HaveField(string expectedFieldName)
   {
      var property = Subject.Fields.FirstOrDefault(p => p.MemberName == expectedFieldName);
      Assert.IsNotNull(property);

      return new SetupMemberModelAssertion(property);
   }

   #endregion

   internal class SetupMemberModelAssertion : ReferenceTypeAssertions<SetupMemberModel, SetupMemberModelAssertion>
   {
      #region Constructors and Destructors

      public SetupMemberModelAssertion(SetupMemberModel subject)
         : base(subject)
      {
      }

      #endregion

      #region Properties

      protected override string Identifier => nameof(SetupMemberModelAssertion);

      #endregion

      #region Public Methods and Operators

      public AndConstraint<SetupMemberModelAssertion> WithSetupMethodName(string expectedName)
      {
         Assert.AreEqual(expectedName, Subject.SetupMethodName);
         return new AndConstraint<SetupMemberModelAssertion>(this);
      }

      #endregion

      public AndConstraint<SetupMemberModelAssertion> WithTypeName(string expectedTypeName)
      {
         Assert.AreEqual(expectedTypeName, Subject.TypeName);
         return new AndConstraint<SetupMemberModelAssertion>(this);
      }

      public AndConstraint<SetupMemberModelAssertion> WithRequiredNamespace(string expectedNamespace)
      {
         Assert.AreEqual(expectedNamespace, Subject.RequiredNamespace);
         return new AndConstraint<SetupMemberModelAssertion>(this);
      }

   }


}