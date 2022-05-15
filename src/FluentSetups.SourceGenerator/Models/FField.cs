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
   using Microsoft.CodeAnalysis.CSharp.Syntax;

   [DebuggerDisplay("{ToCode()}")]
   internal class FField : IFluentTypedMember
   {
      #region Constants and Fields

      private readonly IFieldSymbol fieldSymbol;

      private readonly AttributeData memberAttribute;

      private bool generateFluentSetup;

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
         ComputeDefaultValue();
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

      #region IFluentTypedMember Members

      public bool IsUserDefined => fieldSymbol != null;

      public string ToCode()
      {
         return $"private {TypeName} {Name};";
      }

      public string Name { get; }

      public ITypeSymbol Type { get; }

      /// <summary>Gets a value indicating whether this instance has default value.</summary>
      public bool HasDefaultValue { get; private set; }

      #endregion

      #region Public Properties

      public string DefaultValue { get; private set; }

      public bool GenerateFluentSetup
      {
         get
         {
            if (generateFluentSetup)
               return true;

            if (memberAttribute == null)
               return false;
            return true;
         }
      }

      public string RequiredNamespace { get; set; }

      public string SetupMethodName { get; set; }

      public string TypeName { get; set; }

      #endregion

      #region Public Methods and Operators

      public static FField ForConstructorParameter(IParameterSymbol parameterSymbol)
      {
         return new FField(parameterSymbol.Type, parameterSymbol.Name.ToFirstLower()) { generateFluentSetup = true };
      }

      public static FField ForProperty(FTargetProperty property)
      {
         return new FField(property.Type, property.Name.ToFirstLower()) { generateFluentSetup = true };
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj))
            return false;
         if (ReferenceEquals(this, obj))
            return true;
         if (obj.GetType() != GetType())
            return false;
         return Equals((FField)obj);
      }

      public override int GetHashCode()
      {
         return (Name != null ? Name.GetHashCode() : 0);
      }

      #endregion

      #region Methods

      protected static string ComputeSetupNameFromAttribute(AttributeData attributeData)
      {
         if (attributeData?.AttributeClass == null)
            return null;

         return attributeData.ConstructorArguments.FirstOrDefault().Value?.ToString();
      }

      protected bool Equals(FField other)
      {
         return Name == other.Name;
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

      private void ComputeDefaultValue()
      {
         if (fieldSymbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is VariableDeclaratorSyntax fieldSyntax)
         {
            if (fieldSyntax.Initializer?.Value is LiteralExpressionSyntax literalExpression)
            {
               HasDefaultValue = true;
               DefaultValue = literalExpression.ToString();
            }
         }
      }

      #endregion
   }
}