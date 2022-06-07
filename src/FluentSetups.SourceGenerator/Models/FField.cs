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

      private ITypeSymbol elementType;

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
         IsListMember = IsList(Type);
      }

      public FField(ITypeSymbol type, string name)
      {
         Type = type ?? throw new ArgumentNullException(nameof(type));
         Name = name ?? throw new ArgumentNullException(nameof(name));

         TypeName = Type.ToString();
         SetupMethodName = $"With{WithUpperCase(name)}";
         RequiredNamespace = type.ContainingNamespace.IsGlobalNamespace ? null : type.ContainingNamespace.ToString();
         IsListMember = IsList(Type);
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

      public bool IsListMember { get; }

      public ITypeSymbol ElementType => elementType ?? (elementType = ComputeElementType());

      private ITypeSymbol ComputeElementType()
      {
         if (Type is INamedTypeSymbol namedType && namedType.TypeArguments.Length == 1)
            return namedType.TypeArguments[0];
         return null;
      }

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

      public string TypeName { get; }

      #endregion

      #region Public Methods and Operators

      public static FField ForConstructorParameter(IParameterSymbol parameterSymbol, FluentGeneratorContext context)
      {
         if (TryMapToList(parameterSymbol.Type, context,  out var mappedType))
            return new FField(mappedType, parameterSymbol.Name.ToFirstLower()) { generateFluentSetup = true };
         return new FField(parameterSymbol.Type, parameterSymbol.Name.ToFirstLower()) { generateFluentSetup = true };
      }

      public static FField ForTargetProperty(FTargetProperty property, FluentGeneratorContext context)
      {
         if (TryMapToList(property.Type, context, out var mappedType))
            return new FField(mappedType, property.Name.ToFirstLower()) { generateFluentSetup = true };
         return new FField(property.Type, property.Name.ToFirstLower()) { generateFluentSetup = true };
      }

      private static bool TryMapToList(ITypeSymbol type, FluentGeneratorContext context, out INamedTypeSymbol mappedField)
      {
         mappedField = null;
         if (type is INamedTypeSymbol namedType && IsEnumerable(type))
         {
            if (namedType.TypeArguments.Length != 1)
            {
               return false;
            }

            var genericList = context.Compilation.GetTypeByMetadataName("System.Collections.Generic.List`1");
            if (genericList == null)
               return false;

            mappedField = genericList.Construct(namedType.TypeArguments[0]);
         }

         return mappedField != null;
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

      private static bool IsList(ITypeSymbol typeSymbol)
      {
         if (typeSymbol.ContainingNamespace.ToString() != "System.Collections.Generic")
            return false;

         if (typeSymbol.Name.StartsWith("IList"))
            return true;

         if (typeSymbol.Name.StartsWith("List"))
            return true;

         if (typeSymbol.Name.StartsWith("IEnumerable"))
            return true;

         return false;
      }

      private static bool IsEnumerable(ITypeSymbol typeSymbol)
      {
         if (typeSymbol.ContainingNamespace.ToString() != "System.Collections.Generic")
            return false;

         if (typeSymbol.Name.StartsWith("IEnumerable"))
            return true;

         return false;
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
            if (fieldSyntax.Initializer != null)
            {
               HasDefaultValue = true;
               DefaultValue = fieldSyntax.Initializer.ToString();
            }
         }
      }

      #endregion
   }
}