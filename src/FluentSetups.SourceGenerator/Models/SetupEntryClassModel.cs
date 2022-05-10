// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupEntryClassModel.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System.Collections.Generic;
   using System.Diagnostics;

   [DebuggerDisplay("{Modifier} class {ClassName} int {ContainingNamespace}")]
   internal class SetupEntryClassModel
   {
      #region Public Properties

      /// <summary>Gets or sets the name of the fluent entry class.</summary>
      public string ClassName { get; set; }

      /// <summary>Gets or sets the namespace the fluent entry class will be generated into.</summary>
      public string ContainingNamespace { get; set; }

      /// <summary>Gets or sets the modifier.</summary>
      public string Modifier { get; set; }

      /// <summary>Gets or sets all the <see cref="FClass"/>s that will be accessible from this entry class.</summary>
      public IReadOnlyList<FClass> SetupClasses { get; set; }

      #endregion
   }
}