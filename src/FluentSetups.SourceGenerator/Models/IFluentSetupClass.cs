// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFluentSetupClass.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   /// <summary>A class that defines a fluent setup</summary>
   /// <seealso cref="FluentSetups.SourceGenerator.Models.IFluentClass" />
   internal interface IFluentSetupClass : IFluentClass
   {
      /// <summary>Gets the entry method name, the setup class will be reached with.</summary>
      string EntryMethod { get; }
   }
}