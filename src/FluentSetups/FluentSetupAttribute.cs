// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentSetupAttribute.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups
{
   using System;

   [AttributeUsage(AttributeTargets.Class)]
   public class FluentSetupAttribute : Attribute
   {
      #region Constructors and Destructors

      public FluentSetupAttribute(string setupClassName)
      {
         SetupClassName = setupClassName ?? throw new ArgumentNullException(nameof(setupClassName));
      }

      public FluentSetupAttribute()
         : this("Setup")
      {
      }

      #endregion

      #region Public Properties

      /// <summary>Gets or sets the namespace of the entry class, through which this class will be available.</summary>
      public string EntryNamespace { get; set; }

      /// <summary>Gets the name of the setup class this class is generated inside.</summary>
      public string SetupClassName { get; }

      #endregion
   }
}