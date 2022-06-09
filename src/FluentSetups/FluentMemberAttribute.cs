// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentMemberAttribute.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using JetBrains.Annotations;

namespace FluentSetups
{
   using System;

   [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
   [UsedImplicitly]
   public class FluentMemberAttribute : Attribute
   {
      #region Constructors and Destructors

      public FluentMemberAttribute()
      {
      }

      public FluentMemberAttribute(string fluentMethodName)
      {
         FluentMethodName = fluentMethodName;
      }

      #endregion

      #region Public Properties

      [UsedImplicitly]
      public string FluentMethodName {  get; }

      #endregion
   }
}