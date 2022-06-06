// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoomSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using FluentSetups.IntegrationTests.Targets;

namespace FluentSetups.IntegrationTests.Setups
{
    [FluentSetup(typeof(Room))]
    public partial class RoomSetup
    {
        public RoomSetup WithPerson(Person person)
        {
            throw new System.NotImplementedException();
        }
    }
}
