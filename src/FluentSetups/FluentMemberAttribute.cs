// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentMemberAttribute.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups
{
   using System;

   [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
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

      public string FluentMethodName { get; }

      #endregion
   }
}