// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Olga.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace FluentSetups.IntegrationTests.Targets;


record Spider(IEnumerable<string> Legs);

[FluentSetup(typeof(Spider))]
public partial class SpiderSetup
{
    [FluentMember]
    private List<string> legs;
}

public class Olga
{
   public string LastName { get; set; }
}
