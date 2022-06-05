// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneratorMode.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;

   [Flags]
   internal enum GeneratorMode
   {
      /// <summary>Nothing is generated for this class</summary>
      None = 0x0,

      /// <summary>The setup class is generated</summary>
      Setup = 0x1,

      /// <summary>The entry method is generated</summary>
      EntryMethod = 0x2,

      /// <summary>The setup class and the entry method is generated</summary>
      SetupAndEntryMethod = Setup | EntryMethod
   }
}