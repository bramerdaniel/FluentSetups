// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Setup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.Setups;

internal class Setup
{
   public static SetupClassInfoSetup SetupClassInfo()
   {
      return new SetupClassInfoSetup();
   }
}