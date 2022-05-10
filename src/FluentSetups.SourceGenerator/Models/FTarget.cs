// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FTarget.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System.Collections.Generic;
   using System.Linq;

   using Microsoft.CodeAnalysis;

   internal class FTarget
   {
      #region Constructors and Destructors

      public IReadOnlyList<FTargetProperty> Properties { get; }

      internal FTarget(FClass setupClass, INamedTypeSymbol typeSymbol)
      {
         SetupClass = setupClass;
         TypeSymbol = typeSymbol;
         Properties = ComputeMembers().ToArray();
      }

      private IEnumerable<FTargetProperty> ComputeMembers()
      {
         foreach (var targetMember in TypeSymbol.GetMembers().OfType<IPropertySymbol>())
         {
            if (CanBeSet(targetMember))
               yield return new FTargetProperty(targetMember);
         }
      }

      private static bool CanBeSet(IPropertySymbol targetMember)
      {
         // TODO what about InternalsVisibleTo attribute
         if (targetMember.DeclaredAccessibility != Accessibility.Public)
            return false;

         if (targetMember.IsReadOnly)
            return false;

         if (targetMember.SetMethod == null)
            return false;

         if (targetMember.SetMethod.DeclaredAccessibility != Accessibility.Public)
            return false;

         return true;
      }

      #endregion

      #region Public Properties

      public string Namespace => TypeSymbol.ContainingNamespace.IsGlobalNamespace
            ? null : TypeSymbol.ContainingNamespace.ToString();

      public FClass SetupClass { get; }

      public string TypeName => TypeSymbol.Name;

      public INamedTypeSymbol TypeSymbol { get; }

      #endregion

      #region Public Methods and Operators

      public static FTarget Create(FClass owningSetup, INamedTypeSymbol targetType)
      {
         return new FTarget(owningSetup, targetType);
      }

      #endregion
   }
}