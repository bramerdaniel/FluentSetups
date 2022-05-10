// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFluentMember.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   internal interface IFluentMember
   {
      /// <summary>Gets the name.</summary>
      string Name { get; }

      string ToCode();

      bool IsUserDefined { get; }
   }
}