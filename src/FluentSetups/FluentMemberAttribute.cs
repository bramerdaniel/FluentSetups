// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentMemberAttribute.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
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