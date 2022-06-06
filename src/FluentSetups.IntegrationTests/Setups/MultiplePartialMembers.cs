// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiplePartialMembers.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace FluentSetups.IntegrationTests.Setups;

[FluentSetup]
[SuppressMessage("IDE", "IDE0044:Add readonly modifier")]
[SuppressMessage("IDE", "IDE0052:Remove unused private members")]
public partial class MultiplePartialMembers
{
    [FluentMember]
#pragma warning disable CS0414
    private string value = string.Empty;
#pragma warning restore CS0414
}

public partial class MultiplePartialMembers
{
}
