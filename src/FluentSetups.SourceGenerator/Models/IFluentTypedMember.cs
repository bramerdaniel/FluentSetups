// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFluentTypedMember.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using Microsoft.CodeAnalysis;

   interface IFluentTypedMember : IFluentMember
   {
      /// <summary>Gets the type of the member.</summary>
      ITypeSymbol Type { get;  }

      /// <summary>Gets a value indicating whether this member has default value.</summary>
      bool HasDefaultValue { get; }

      /// <summary>Gets a value indicating whether this member is a sort of list.</summary>
      bool IsListMember { get; }

      /// <summary>Gets the type of the element.</summary>
      ITypeSymbol ElementType { get; }
   }
}