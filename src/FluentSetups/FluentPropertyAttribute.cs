// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentPropertyAttribute.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups
{
   using System;

   [AttributeUsage(AttributeTargets.Property)]
   public class FluentPropertyAttribute : Attribute
   {
      #region Constructors and Destructors

      public FluentPropertyAttribute()
      {
      }

      public FluentPropertyAttribute(string fluentMethodName)
      {
         FluentMethodName = fluentMethodName;
      }

      #endregion

      #region Public Properties

      public string FluentMethodName { get; }

      #endregion
   }
}