// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetMode.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using JetBrains.Annotations;

namespace FluentSetups
{
   public enum TargetMode
   {
      /// <summary>The fluent setup engine will decide whether it can create the target or not.</summary>
      [UsedImplicitly] Automatic,

      /// <summary>No methods for creating the setup target object will be generated.</summary>
      [UsedImplicitly] Disabled
   }
}