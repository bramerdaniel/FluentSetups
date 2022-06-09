// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FProperty.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Diagnostics;
   using System.Linq;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp.Syntax;

   [DebuggerDisplay("{TypeName} {Name}")]
   internal class FProperty : IFluentTypedMember
   {
      #region Constants and Fields

      private readonly AttributeData memberAttribute;

      private readonly IPropertySymbol propertySymbol;

      #endregion

      #region Constructors and Destructors

      public FProperty(IPropertySymbol propertySymbol, AttributeData memberAttribute)
      {
         this.propertySymbol = propertySymbol ?? throw new ArgumentNullException(nameof(propertySymbol));
         this.memberAttribute = memberAttribute;
         Name = propertySymbol.Name;
         ComputeDefaultValue();
      }
      
      private void ComputeDefaultValue()
      {
         if (propertySymbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is PropertyDeclarationSyntax propertySyntax)
         {
            if (propertySyntax.Initializer?.Value is LiteralExpressionSyntax literalExpression)
            {
               HasDefaultValue = true;
            }
         }
      }

      #endregion

      #region IFluentTypedMember Members

      public ITypeSymbol Type => propertySymbol.Type;

      // TODO handel this
      public bool HasDefaultValue { get; private set; }

      public bool IsListMember { get; }

      public ITypeSymbol ElementType { get; }

      public string Name { get; set; }

      public string ToCode()
      {
         return "// NOT IMPLEMENTED";
      }

      public bool IsUserDefined => propertySymbol != null;

      #endregion

      #region Public Properties

      public string RequiredNamespace => ComputeRequiredNamespace(propertySymbol);

      public string TypeName => Type.ToString();

      #endregion

      #region Public Methods and Operators

      public string GetSetupMethodName()
      {
         return ComputeSetupNameFromAttribute(memberAttribute) ?? $"With{propertySymbol.Name}";
      }

      public bool RequiredSetupGeneration()
      {
         if (memberAttribute == null)
            return false;
         return true;
      }

      #endregion

      #region Methods

      protected static string ComputeSetupNameFromAttribute(AttributeData attributeData)
      {
         if (attributeData == null)
            return null;

         return attributeData.ConstructorArguments.FirstOrDefault().Value?.ToString();
      }

      private static string ComputeRequiredNamespace(IPropertySymbol propertySymbol)
      {
         return propertySymbol.Type.ContainingNamespace.IsGlobalNamespace ? null : propertySymbol.Type.ContainingNamespace.ToString();
      }

      #endregion
   }
}