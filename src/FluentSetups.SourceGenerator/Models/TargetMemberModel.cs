// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetMemberModel.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;

   using Microsoft.CodeAnalysis;

   internal class TargetMemberModel : FMember
   {
      public IPropertySymbol PropertySymbol { get; }

      private TargetMemberModel(IPropertySymbol propertySymbol)
      {
         PropertySymbol = propertySymbol ?? throw new ArgumentNullException(nameof(propertySymbol));
         MemberName = PropertySymbol.Name;
         SetupMethodName = $"With{propertySymbol.Name}";
         TypeName = propertySymbol.Type.ToString();
         RequiredNamespace = propertySymbol.Type.ContainingNamespace.ToString(); // TODO for global namespace
         
      }

      public static TargetMemberModel Create(IPropertySymbol propertySymbol)
      {
         return new TargetMemberModel(propertySymbol);
      }
   }
}