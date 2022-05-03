// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Setup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.Setups;

internal class Setup
{
   #region Public Methods and Operators

   public static SetupClassInfoSetup SetupClassInfo()
   {
      return new SetupClassInfoSetup();
   }

   public static SourceGeneratorTestSetup SourceGeneratorTest()
   {
      return new SourceGeneratorTestSetup();
   }

   #endregion

   public static SetupClassModelSetup SetupClassModel()
   {
      return new SetupClassModelSetup();
   }
}