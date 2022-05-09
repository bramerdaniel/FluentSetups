// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FMember.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System.Linq;

   using Microsoft.CodeAnalysis;

   internal class FMember
   {
      #region Public Properties

      public string LowerMemberName => $"{char.ToLowerInvariant(MemberName[0])}{MemberName.Substring(1)}";

      /// <summary>Gets or sets the name of the member.</summary>
      public string MemberName { get; set; }

      public string MemberSetFieldName => $"{LowerMemberName}WasSet";

      public string RequiredNamespace { get; protected set; }

      /// <summary>Gets or sets the name of the setup method.</summary>
      public string SetupMethodName { get; protected set; }

      public string TypeName { get; protected set; }

      public string UpperMemberName => $"{char.ToUpperInvariant(MemberName[0])}{MemberName.Substring(1)}";

      #endregion

      #region Methods

      protected static string ComputeSetupNameFromAttribute(AttributeData attributeData)
      {
         return attributeData.ConstructorArguments.FirstOrDefault().Value?.ToString();
      }

      #endregion
   }
}