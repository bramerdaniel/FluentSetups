// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FField.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Diagnostics;
   using System.Linq;

   using Microsoft.CodeAnalysis;

   [DebuggerDisplay("private {TypeName} {Name}")]
   internal class FField : IFluentTypedMember
   {
      #region Constants and Fields

      private readonly IFieldSymbol fieldSymbol;

      private readonly AttributeData memberAttribute;

      #endregion

      #region Constructors and Destructors

      public FField(IFieldSymbol fieldSymbol, AttributeData memberAttribute)
      {
         this.fieldSymbol = fieldSymbol ?? throw new ArgumentNullException(nameof(fieldSymbol));
         this.memberAttribute = memberAttribute;

         Name = fieldSymbol.Name;
         Type = fieldSymbol.Type;
         TypeName = fieldSymbol.Type.ToString();
         SetupMethodName = ComputeSetupNameFromAttribute(memberAttribute) ?? $"With{WithUpperCase(fieldSymbol)}";
         RequiredNamespace = ComputeRequiredNamespace(fieldSymbol);
      }

      public FField(ITypeSymbol type, string name)
      {
         Type = type ?? throw new ArgumentNullException(nameof(type));
         Name = name ?? throw new ArgumentNullException(nameof(name));

         TypeName = Type.ToString();
         SetupMethodName = $"With{WithUpperCase(name)}";
         RequiredNamespace = type.ContainingNamespace.IsGlobalNamespace ? null : type.ContainingNamespace.ToString();
      }

      #endregion

      #region IFluentMember Members

      public bool IsUserDefined => fieldSymbol != null;

      public string ToCode()
      {
         return $"private {TypeName} {Name};";
      }

      #endregion

      #region Public Properties

      public string Name { get; set; }

      public string RequiredNamespace { get; set; }

      public string SetupMethodName { get; set; }

      public ITypeSymbol Type { get;  }

      public string TypeName { get; set; }

      #endregion

      #region Public Methods and Operators

      public static FField ForProperty(FTargetProperty property)
      {
         return new FField(property.Type, property.Name.ToFirstLower());
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
         if (attributeData?.AttributeClass == null)
            return null;

         return attributeData.ConstructorArguments.FirstOrDefault().Value?.ToString();
      }

      private static string ComputeRequiredNamespace(IFieldSymbol fieldSymbol)
      {
         return fieldSymbol.Type.ContainingNamespace.IsGlobalNamespace ? null : fieldSymbol.Type.ContainingNamespace.ToString();
      }

      private static string WithUpperCase(IFieldSymbol fieldSymbol)
      {
         return $"{char.ToUpper(fieldSymbol.Name[0])}{fieldSymbol.Name.Substring(1)}";
      }

      private static string WithUpperCase(string name)
      {
         return $"{char.ToUpper(name[0])}{name.Substring(1)}";
      }

      #endregion
   }
}