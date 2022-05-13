// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentSetupAttribute.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups
{
   using System;

   [AttributeUsage(AttributeTargets.Class)]
   public class FluentSetupAttribute : Attribute
   {
      #region Constructors and Destructors

      /// <summary>Initializes a new instance of the <see cref="FluentSetupAttribute"/> class.</summary>
      /// <param name="entryClassName">The name of the class the created fluent setup should be accessible with, or better the fluent entry point.</param>
      public FluentSetupAttribute(string entryClassName)
      {
         EntryClassName = entryClassName ?? throw new ArgumentNullException(nameof(entryClassName));
      }

      /// <summary>Initializes a new instance of the <see cref="FluentSetupAttribute"/> class.</summary>
      /// <param name="targetType">Type of the target object that should be created.</param>
      public FluentSetupAttribute(Type targetType)
         : this("Setup")
      {
         TargetType = targetType;
      }

      public FluentSetupAttribute()
         : this("Setup")
      {
      }

      #endregion

      #region Public Properties

      /// <summary>Gets the name of the setup class this class is generated inside.</summary>
      public string EntryClassName { get; set; }

      /// <summary>Gets or sets the namespace of the entry class, through which this class will be available.</summary>
      public string EntryNamespace { get; set; }

      /// <summary>Gets or sets the type of the target.</summary>
      public Type TargetType { get; set; }

      /// <summary>Gets or sets the mode how the fluent setup generator will create the functions for generating the fluent setup target object.</summary>
      public TargetMode TargetMode { get; set; }

      #endregion
   }
}