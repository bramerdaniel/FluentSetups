// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupClassInfo.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using System;
   using System.Linq;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp.Syntax;

   /// <summary>Data class containing all the required information along the generation process</summary>
   internal class SetupClassInfo
   {
      #region Constructors and Destructors

      public SetupClassInfo(ClassDeclarationSyntax candidate, SemanticModel semanticModel)
      {
         ClassSyntax = candidate ?? throw new ArgumentNullException(nameof(candidate));
         SemanticModel = semanticModel ?? throw new ArgumentNullException(nameof(semanticModel));
      }

      #endregion

      #region Public Properties

      public string ClassName => ClassSyntax.Identifier.Text;

      /// <summary>Gets or sets the class symbol.</summary>
      public ITypeSymbol ClassSymbol { get; set; }

      public ClassDeclarationSyntax ClassSyntax { get; }

      public AttributeData FluentSetupAttribute { get; set; }

      public SemanticModel SemanticModel { get; }

      #endregion

      #region Methods

      internal string GetSetupEntryNameSpace()
      {
         var firstArgument = FluentSetupAttribute.NamedArguments.FirstOrDefault(x => x.Key == "EntryNamespace");
         if (firstArgument.Value.Value is string value)
            return value;
         return ClassSymbol.ContainingNamespace.ToString();
      }

      internal string GetSetupEntryClassName()
      {
         var firstArgument = FluentSetupAttribute.ConstructorArguments.FirstOrDefault();
         if (firstArgument.IsNull)
            return "Setup";

         return firstArgument.Value?.ToString() ?? "Setup";
      }

      #endregion
   }
}