﻿// --------------------------------------------------------------------------------------------------------------------
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
   }
}