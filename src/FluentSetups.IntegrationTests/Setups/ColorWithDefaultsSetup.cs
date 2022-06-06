// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorWithDefaultsSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using FluentSetups.IntegrationTests.Targets;

namespace FluentSetups.IntegrationTests.Setups
{
#pragma warning disable CS0414
    [FluentSetup(typeof(Color), SetupMethod = "ColorWithDefaults")]
    public partial class ColorWithDefaultsSetup
    {
        private bool nameWasSet;

        [FluentMember]
        private string name = string.Empty;

        [FluentMember]
        private int opacity = InitOpacity();

        private static int InitOpacity()
        {
            return -1;
        }
    }
#pragma warning restore CS0414
}
