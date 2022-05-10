// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FProperty.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Linq;

   using Microsoft.CodeAnalysis;

   internal class FProperty : IFluentTypedMember
   {
      private readonly IPropertySymbol propertySymbol;

      private readonly AttributeData memberAttribute;

      public FProperty(IPropertySymbol propertySymbol, AttributeData memberAttribute)
      {
         this.propertySymbol = propertySymbol ?? throw new ArgumentNullException(nameof(propertySymbol));
         this.memberAttribute = memberAttribute;
         Name = propertySymbol.Name;
      }

      protected static string ComputeSetupNameFromAttribute(AttributeData attributeData)
      {
         if (attributeData == null)
            return null;

         return attributeData.ConstructorArguments.FirstOrDefault().Value?.ToString();
      }

      public ITypeSymbol Type => propertySymbol.Type;

      #region Public Properties

      #endregion

      #region Public Methods and Operators

      public string Name { get; set; }

      private static string ComputeRequiredNamespace(IPropertySymbol propertySymbol)
      {
         return propertySymbol.Type.ContainingNamespace.IsGlobalNamespace ? null : propertySymbol.Type.ContainingNamespace.ToString();
      }

      #endregion

      #region Methods

      #endregion

      public bool RequiredSetupGeneration()
      {
         if (memberAttribute == null)
            return false;
         return true;
      }

      public string ToCode()
      {
         return "// NOT IMPLEMENTED";
      }

      public bool IsUserDefined => propertySymbol != null;

      public string TypeName => Type.ToString();

      public string RequiredNamespace => ComputeRequiredNamespace(propertySymbol);

      public string GetSetupMethodName()
      {
         return ComputeSetupNameFromAttribute(memberAttribute) ?? $"With{propertySymbol.Name}";
      }
   }
}