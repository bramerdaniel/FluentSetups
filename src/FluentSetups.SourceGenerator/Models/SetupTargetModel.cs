// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupTargetModel.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System.Collections.Generic;
   using System.Globalization;
   using System.Linq;

   using Microsoft.CodeAnalysis;

   internal class SetupTargetModel
   {
      #region Constructors and Destructors

      public IReadOnlyList<TargetMemberModel> Members { get; }

      private SetupTargetModel(SetupClassModel setupClass, INamedTypeSymbol typeSymbol)
      {
         SetupClass = setupClass;
         TypeSymbol = typeSymbol;
         Members = ComputeMembers(typeSymbol).ToArray();
      }

      private static IEnumerable<TargetMemberModel> ComputeMembers(INamedTypeSymbol typeSymbol)
      {
         foreach (var targetMember in typeSymbol.GetMembers().OfType<IPropertySymbol>())
         {
            if (CanBeSet(targetMember))
               yield return TargetMemberModel.Create(targetMember);
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

         return true;
      }

      #endregion

      #region Public Properties

      public string Namespace => TypeSymbol.ContainingNamespace.IsGlobalNamespace
            ? null : TypeSymbol.ContainingNamespace.ToString();

      public SetupClassModel SetupClass { get; }

      public string TypeName => TypeSymbol.Name;

      public INamedTypeSymbol TypeSymbol { get; }

      #endregion

      #region Public Methods and Operators

      public static SetupTargetModel Create(SetupClassModel owningSetup, INamedTypeSymbol targetType)
      {
         return new SetupTargetModel(owningSetup, targetType);
      }

      #endregion
   }
}