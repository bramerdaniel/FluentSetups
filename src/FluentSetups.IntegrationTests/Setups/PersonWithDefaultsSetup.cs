// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonWithDefaultsSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using FluentSetups.IntegrationTests.Targets;

namespace FluentSetups.IntegrationTests.Setups
{
    [FluentSetup(typeof(Person), SetupMethod = "PersonWithDefaultName")]
    public partial class PersonWithDefaultsSetup
    {
        #region Constants and Fields

        [FluentMember]
        private string firstName = "John";

        [FluentMember]
        private string lastName = "Doe";

        #endregion
    }
}
