// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Setups;

using FluentSetups.IntegrationTests.Targets;

[FluentSetup(typeof(Person))]
internal partial class PersonSetup 
{
   [FluentMember]
   public string FirstName { get; set; }

}