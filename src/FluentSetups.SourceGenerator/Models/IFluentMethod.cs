﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFluentMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   interface IFluentMethod : IFluentMember
   {
      /// <summary>Gets the methods parameter count.</summary>
      int ParameterCount { get; }
   }
}