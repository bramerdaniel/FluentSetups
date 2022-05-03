// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupPropertyModel.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using Microsoft.CodeAnalysis;

   internal class SetupPropertyModel : SetupMemberModel
   {
      #region Constructors and Destructors

      public SetupPropertyModel(SetupClassModel owningClass)
         : base(owningClass)
      {
      }

      #endregion

      #region Public Properties
      
      #endregion

      #region Public Methods and Operators

      public static SetupPropertyModel Create(SetupClassModel owningClass, IPropertySymbol propertySymbol, AttributeData attribute)
      {
         return new SetupPropertyModel(owningClass)
         {
            MemberName = propertySymbol.Name, 
            TypeName = propertySymbol.Type.ToString(),
            SetupMethodName = ComputeSetupNameFromAttribute(attribute) ?? $"With{propertySymbol.Name}",
            RequiredNamespace = ComputeRequiredNamespace(propertySymbol)
         };
      }

      private static string ComputeRequiredNamespace(IPropertySymbol propertySymbol)
      {
         return propertySymbol.Type.ContainingNamespace.IsGlobalNamespace ? null : propertySymbol.Type.ContainingNamespace.ToString();
      }

      #endregion

      #region Methods

      #endregion
   }
}