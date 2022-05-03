// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupEntryClassModel.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System.Collections.Generic;
   using System.Diagnostics;

   [DebuggerDisplay("{Modifier} class {ClassName} int {ContainingNamespace}")]
   internal class SetupEntryClassModel
   {
      public string ClassName { get; set; }

      public string ContainingNamespace { get; set; }

      public string Modifier { get; set; } = "public";

      public IList<SetupClassModel> SetupClasses { get; set; }
   }
}