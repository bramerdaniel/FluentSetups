// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HideMethodColorSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Setups;

using FluentSetups.IntegrationTests.Targets;

[FluentSetup(typeof(Color))]
public partial class HideMethodColorSetup
{


   public HideMethodColorSetup WithName(string value)
   {
      // This method hides the generated one => it is not generated
      return this;
   }

   public HideMethodColorSetup WithOpacity(string value)
   {
      // This is an overload
      return WithOpacity(int.Parse(value));
   }
}