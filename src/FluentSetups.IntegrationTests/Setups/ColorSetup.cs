// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Setups;

using System.Diagnostics.CodeAnalysis;

using FluentSetups.IntegrationTests.Targets;

[FluentSetup(typeof(Color))]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public partial class ColorSetup
{
   [FluentMember]
   private string name;

   public ColorSetup WithName(string value)
   {
      // This method hides the generated one => it is not generated
      name = value;
      return this;
   }

   public ColorSetup WithOpacity(string value)
   {
      // This is an overload
      return WithOpacity(int.Parse(value));
   }
}