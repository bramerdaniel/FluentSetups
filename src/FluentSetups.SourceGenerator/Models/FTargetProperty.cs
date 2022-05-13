// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FTargetProperty.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;

   using Microsoft.CodeAnalysis;

   internal class FTargetProperty
   {
      public IPropertySymbol PropertySymbol { get; }

      internal FTargetProperty(IPropertySymbol propertySymbol)
      {
         PropertySymbol = propertySymbol ?? throw new ArgumentNullException(nameof(propertySymbol));
         Name = PropertySymbol.Name;
         SetupMethodName = $"With{propertySymbol.Name}";
         RequiredNamespace = propertySymbol.Type.ContainingNamespace.ToString(); // TODO for global namespace
      }

      public string RequiredNamespace { get; }

      public string SetupMethodName { get; }

      public string TypeName => PropertySymbol.Type.ToString();

      public string Name { get; set; }

      public ITypeSymbol Type => PropertySymbol.Type;

      public static FTargetProperty Create(IPropertySymbol propertySymbol)
      {
         return new FTargetProperty(propertySymbol);
      }
   }
}