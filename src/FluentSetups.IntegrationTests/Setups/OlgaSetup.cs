// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OlgaSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Setups
{
   using FluentSetups.IntegrationTests.Targets;

   [FluentSetup(typeof(Olga))]
   public partial class OlgaSetup
   {
      private void SetupTarget(Olga target)
      {
         target.LastName = "OlgaSetup";
      }
   }

   [FluentSetup(typeof(Olga))]    
   public partial class AnotherOlgaSetup
   {
      private void SetupTarget(Olga target)
      {
         target.LastName = "AnotherOlgaSetup";
      }
   }
}