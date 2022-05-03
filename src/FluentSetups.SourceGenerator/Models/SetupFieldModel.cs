// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupFieldModel.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using Microsoft.CodeAnalysis;

   internal class SetupFieldModel : SetupMemberModel
   {
      #region Constructors and Destructors

      public SetupFieldModel(SetupClassModel owningClass)
         : base(owningClass)
      {
      }

      #endregion

      #region Public Properties
      

      #endregion

      #region Public Methods and Operators

      public static SetupFieldModel Create(SetupClassModel owningClass, IFieldSymbol fieldSymbol, AttributeData attribute)
      {
         return new SetupFieldModel(owningClass)
         {
            MemberName = fieldSymbol.Name, 
            TypeName = fieldSymbol.Type.ToString(),
            SetupMethodName = ComputeSetupNameFromAttribute(attribute) ?? $"With{WithUpperCase(fieldSymbol)}",
            RequiredNamespace = ComputeRequiredNamespace(fieldSymbol)
         };
      }

      private static string WithUpperCase(IFieldSymbol fieldSymbol)
      {
         return $"{char.ToUpper(fieldSymbol.Name[0])}{fieldSymbol.Name.Substring(1)}";
      }

      private static string ComputeRequiredNamespace(IFieldSymbol fieldSymbol)
      {
         return fieldSymbol.Type.ContainingNamespace.IsGlobalNamespace ? null : fieldSymbol.Type.ContainingNamespace.ToString();
      }

      #endregion

      #region Methods

      #endregion
   }
}