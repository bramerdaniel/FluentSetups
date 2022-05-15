// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoneOverwriteSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Setups;

using FluentSetups.IntegrationTests.Targets;

[FluentSetup(typeof(Person), EntryClassName = "CustomSetup")]
internal partial class DoneOverwriteSetup 
{
   internal Person Done()
   {
      return new Person
      {
         FirstName = GetLastName(null),
         LastName = GetFirstName(null),
         Age = GetAge(() => 10)
      };
   }

}