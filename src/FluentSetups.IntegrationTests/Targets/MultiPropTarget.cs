// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiPropTarget.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Targets;

using System;

public class MultiPropTarget
{
   #region Public Properties

   public int Kind { get; set; }

   public string Name { get; set; }

   public Type Type { get; set; }

   #endregion
}