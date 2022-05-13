// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentEntryNamespaceAttribute.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups
{
   using System;

   [AttributeUsage(AttributeTargets.Assembly)]
   public class FluentEntryNamespaceAttribute : Attribute
   {
      #region Constructors and Destructors

      public FluentEntryNamespaceAttribute(string defaultNamespace)
      {
         EntryNamespace = defaultNamespace ?? throw new ArgumentNullException(nameof(defaultNamespace));
      }

      #endregion

      #region Public Properties

      public string EntryNamespace { get; set; }

      #endregion
   }
}