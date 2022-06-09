// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentSetupAttribute.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    ///     Applied to a partial class, this class will become a fluent setup.
    ///     <code>
    /// [FluentSetup(typeof(Person))]
    /// internal partial class PersonSetup
    /// {
    ///    // the properties of the Person class will be available in the generated part of this class,
    ///    // additional setup member of overriden existing ones can be specified here 
    /// }
    /// </code>
    ///     This means that you will be able to reach it from the setup root class, which can be defined with the <see cref="FluentRootAttribute"/>, or is
    ///     called 'Setup' by default. The used would look like this.
    ///     <code>
    /// var person = Setup.Person()
    ///                   .WithName("Bob")
    ///                   .WithAge(47)
    ///                   .Done();
    /// </code>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [UsedImplicitly]
    public class FluentSetupAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="FluentSetupAttribute"/> class.</summary>
        public FluentSetupAttribute()
            : this("Setup")
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FluentSetupAttribute"/> class.</summary>
        /// <param name="targetType">Type of the target object that should be created.</param>
        public FluentSetupAttribute([NotNull] Type targetType)
            : this("Setup")
        {
            TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
        }

        /// <summary>Initializes a new instance of the <see cref="FluentSetupAttribute"/> class.</summary>
        /// <param name="entryClassName">The name of the class the created fluent setup should be accessible with, or better the fluent entry point.</param>
        public FluentSetupAttribute([NotNull] string entryClassName)
        {
            EntryClassName = entryClassName ?? throw new ArgumentNullException(nameof(entryClassName));
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the name of the setup class this class is generated inside.</summary>
        [UsedImplicitly]
        public string EntryClassName { get; set; }

        /// <summary>Gets or sets the namespace of the entry class, through which this class will be available.</summary>
        [UsedImplicitly]
        public string EntryNamespace { get; set; }

        /// <summary>
        ///     Gets or sets the name of the setup entry method generated in the <see cref="EntryClassName"/>. This is by default the name of the target
        ///     type if specified. <code>&lt;EntryClassName&gt;.&lt;SetupMethod&gt;()</code> e.g. <code>Setup.MyClass()</code>
        /// </summary>
        [UsedImplicitly]
        public string SetupMethod { get; set; }

        /// <summary>Gets or sets the mode how the fluent setup generator will create the functions for generating the fluent setup target object.</summary>
        [UsedImplicitly]
        public TargetMode TargetMode { get; set; }

        /// <summary>Gets or sets the type of the target.</summary>
        [UsedImplicitly]
        public Type TargetType { get; set; }

        #endregion
    }
}