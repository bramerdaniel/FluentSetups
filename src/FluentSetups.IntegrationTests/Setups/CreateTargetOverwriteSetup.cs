// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateTargetOverwriteSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Setups;

using FluentSetups.IntegrationTests.Targets;

[FluentSetup(typeof(Person))]
internal partial class CreateTargetOverwriteSetup 
{
   public bool CreateTargetCalled { get; set; }

   internal Person CreateTarget()
   {
      CreateTargetCalled = true;
      return new Person
      {
         FirstName = GetLastName(null),
         LastName = GetFirstName(null),
         Age = GetAge(() => 10)
      };
   }

}