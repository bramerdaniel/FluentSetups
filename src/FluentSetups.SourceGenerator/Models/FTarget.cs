// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FTarget.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Collections.Generic;
   using System.Linq;

   using Microsoft.CodeAnalysis;

   internal class FTarget
   {
      #region Constructors and Destructors

      public IReadOnlyList<FTargetProperty> Properties { get; }

      internal FTarget(FClass setupClass, INamedTypeSymbol typeSymbol)
      {
         SetupClass = setupClass ?? throw new ArgumentNullException(nameof(setupClass));
         TypeSymbol = typeSymbol ?? throw new ArgumentNullException(nameof(typeSymbol));
         Constructor = GetAccessibleConstructorWithLeastParameters();
         ConstructorParameters = SelectBest().ToArray();
         Properties = ComputeMembers().ToArray();
      }

      /// <summary>Gets the constructor that will be used to create the target object.</summary>
      public IMethodSymbol Constructor { get; }

      public IReadOnlyList<IFluentTypedMember> ConstructorParameters { get; set; }

      private IEnumerable<IFluentTypedMember> SelectBest()
      {
         if (Constructor == null)
            yield break;

         foreach (var parameter in Constructor.Parameters)
            yield return new FConstructorParameter(parameter);
      }

      private IMethodSymbol GetAccessibleConstructorWithLeastParameters()
      {
         foreach (var candidate in TypeSymbol.Constructors.OrderBy(x => x.Parameters.Length))
         {
            if (candidate.DeclaredAccessibility == Accessibility.Public)
               return candidate;
         }

         return null;
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

      public bool IsInternal => TypeSymbol.DeclaredAccessibility == Accessibility.Internal;

      #endregion

      #region Public Methods and Operators

      public static FTarget Create(FClass owningSetup, INamedTypeSymbol targetType)
      {
         return new FTarget(owningSetup, targetType);
      }

      #endregion

      public bool HasAccessibleProperty(string name)
      {
         return Properties.Any(p => p.Name == name);
      }
   }
}