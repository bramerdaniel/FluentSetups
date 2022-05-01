// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupClassInfo.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp.Syntax;

   /// <summary>Data class containing all the required information along the generation process</summary>
   internal class SetupClassInfo
   {
      #region Public Properties

      public SemanticModel ClassModel { get; set; }

      public ClassDeclarationSyntax ClassSyntax { get; set; }

      public AttributeData FluentSetupAttribute { get; set; }

      public string ClassName => ClassSyntax.Identifier.Text;

      #endregion
   }
}