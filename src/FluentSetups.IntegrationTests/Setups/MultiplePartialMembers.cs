// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiplePartialMembers.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Setups;

[FluentSetup]
public partial class MultiplePartialMembers
{
   [FluentMember]
   private string value = "string.Empty";
}

public partial class MultiplePartialMembers
{ 
} 