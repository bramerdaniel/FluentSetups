// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFluentMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using Microsoft.CodeAnalysis;

   interface IFluentMethod : IFluentMember
   {
      /// <summary>Gets the methods parameter count.</summary>
      int ParameterCount { get; }

      string Category { get; }

      /// <summary>Gets the type of the first parameter parameter.</summary>
      ITypeSymbol ParameterType { get; }
   }
}