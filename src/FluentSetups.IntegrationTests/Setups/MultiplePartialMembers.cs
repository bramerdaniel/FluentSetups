// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiplePartialMembers.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace FluentSetups.IntegrationTests.Setups;

#pragma warning disable CS0414
[FluentSetup]
[SuppressMessage("IDE", "IDE0044:Add readonly modifier")]
[SuppressMessage("IDE", "IDE0051:Remove unused private members")]
public partial class MultiplePartialMembers
{
   [FluentMember]
   private string value = "string.Empty";
}

public partial class MultiplePartialMembers
{
}