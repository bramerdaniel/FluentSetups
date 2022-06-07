// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFluentClass.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using Microsoft.CodeAnalysis;

   internal interface IFluentClass
   {
      #region Public Properties

      /// <summary>Gets the name of the class.</summary>
      string ClassName { get; }

      INamedTypeSymbol ClassSymbol { get; }

      string Modifier { get; }

      #endregion
   }
}