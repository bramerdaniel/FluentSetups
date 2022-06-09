// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentEntryNamespaceAttribute.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

using JetBrains.Annotations;

namespace FluentSetups
{
   /// <summary>
   ///    Applied to a class, this class will become the root entry point for all generated setup classes, that do no specify something else.
   ///    <code>
   /// [FluentRoot]
   /// internal partial class Create
   /// {
   ///    // some custom code here is possible
   ///    // in the generated part of this class all entry setup methods will be generated into
   /// }
   /// </code>
   /// </summary>
   [AttributeUsage(AttributeTargets.Class)]
   [UsedImplicitly]
   public class FluentRootAttribute : Attribute
   {
   }
}