// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFluentSetupClass.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   /// <summary>A class that defines a fluent setup</summary>
   /// <seealso cref="FluentSetups.SourceGenerator.Models.IFluentClass"/>
   internal interface IFluentSetupClass : IFluentClass
   {
      #region Public Properties

      /// <summary>Gets the entry method name, the setup class will be reached with.</summary>
      string EntryMethod { get; }

      #endregion
   }
}