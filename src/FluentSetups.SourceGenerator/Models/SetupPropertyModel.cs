// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupPropertyModel.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Linq;

   using Microsoft.CodeAnalysis;

   internal class SetupPropertyModel
   {
      #region Constructors and Destructors

      public SetupPropertyModel(SetupClassModel owningClass)
      {
         OwningClass = owningClass ?? throw new ArgumentNullException(nameof(owningClass));
      }

      #endregion

      #region Public Properties

      /// <summary>Gets the owning class.</summary>
      public SetupClassModel OwningClass { get; }

      public string PropertyName { get; private set; }

      /// <summary>Gets or sets the name of the setup method.</summary>
      public string SetupMethodName { get; private set; }

      public string TypeName { get; private set; }

      public string RequiredNamespace { get; private set; }

      #endregion

      #region Public Methods and Operators

      public static SetupPropertyModel Create(SetupClassModel owningClass, IPropertySymbol propertySymbol, AttributeData attribute)
      {
         return new SetupPropertyModel(owningClass)
         {
            PropertyName = propertySymbol.Name, 
            TypeName = propertySymbol.Type.ToString(),
            SetupMethodName = ComputeSetupNameFromAttribute(attribute) ?? $"With{propertySymbol.Name}",
            RequiredNamespace = ComputeRequiredNamespace(propertySymbol)
         };
      }

      private static string ComputeRequiredNamespace(IPropertySymbol propertySymbol)
      {
         return propertySymbol.Type.ContainingNamespace.IsGlobalNamespace ? null : propertySymbol.Type.ContainingNamespace.ToString();
      }

      #endregion

      #region Methods

      private static string ComputeSetupNameFromAttribute(AttributeData attributeData)
      {
         return attributeData.ConstructorArguments.FirstOrDefault().Value?.ToString();
      }

      #endregion
   }
}