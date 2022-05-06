// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetMode.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups
{
   public enum TargetMode
   {
      /// <summary>The fluent setup engine will decide whether it can create the target or not.</summary>
      Automatic,

      /// <summary>No methods for creating the setup target object will be generated.</summary>
      Disabled
   }
}