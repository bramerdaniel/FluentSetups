// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Room.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace FluentSetups.IntegrationTests.Targets
{
    public record Room(IEnumerable<Person> People);
}
