// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupMemberModel.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Linq;

   using Microsoft.CodeAnalysis;

   internal class SetupMemberModel
   {
      #region Constructors and Destructors

      public SetupMemberModel(SetupClassModel owningClass)
      {
         OwningClass = owningClass ?? throw new ArgumentNullException(nameof(owningClass));
      }

      #endregion

      #region Public Properties

      /// <summary>Gets the owning class.</summary>
      public SetupClassModel OwningClass { get; }

      public string RequiredNamespace { get; protected set; }

      /// <summary>Gets or sets the name of the setup method.</summary>
      public string SetupMethodName { get; protected set; }

      public string TypeName { get; protected set; }

      /// <summary>Gets or sets the name of the member.</summary>
      public string MemberName { get; set; }

      public string UpperMemberName => $"{char.ToUpperInvariant(MemberName[0])}{MemberName.Substring(1)}";
      
      public string LowerMemberName => $"{char.ToLowerInvariant(MemberName[0])}{MemberName.Substring(1)}";

      public string MemberSetFieldName => $"{LowerMemberName}WasSet";

      #endregion

      #region Methods

      protected static string ComputeSetupNameFromAttribute(AttributeData attributeData)
      {
         return attributeData.ConstructorArguments.FirstOrDefault().Value?.ToString();
      }

      #endregion
   }
}