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

internal class SetupClassModelAssertion : ReferenceTypeAssertions<FClass, SetupClassModelAssertion>
{
   #region Constructors and Destructors

   public SetupClassModelAssertion(FClass target)
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

   public AndConstraint<SetupClassModelAssertion> HaveTargetMode(TargetGenerationMode expectedMode)
   {
      Assert.AreEqual(expectedMode, Subject.TargetMode);
      return new AndConstraint<SetupClassModelAssertion>(this);
   }

   public FPropertyAssertion HaveProperty(string expectedPropertyName)
   {
      var property = Subject.Properties.FirstOrDefault(p => p.MemberName == expectedPropertyName);
      Assert.IsNotNull(property);

      return new FPropertyAssertion(property);
   }

   public FMethodAssertion HaveMethod(string expectedFieldName)
   {
      var method = Subject.Methods.FirstOrDefault(p => p.MemberName == expectedFieldName);
      Assert.IsNotNull(method, $"The class {Subject.ClassName} did not have the expected method '{expectedFieldName}'.");

      return new FMethodAssertion(method);
   }
   public FFieldAssertion HaveField(string expectedFieldName)
   {
      var field = Subject.Fields.FirstOrDefault(p => p.Name == expectedFieldName);
      Assert.IsNotNull(field, $"The class {Subject.ClassName} did not have the expected field {expectedFieldName}.");

      return new FFieldAssertion(field);
   }

   #endregion



}