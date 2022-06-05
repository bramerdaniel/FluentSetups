// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorWithDefaultsSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using FluentSetups.IntegrationTests.Targets;

namespace FluentSetups.IntegrationTests.Setups
{
    [FluentSetup(typeof(Color), SetupMethod = "ColorWithDefaults")]
    public partial class ColorWithDefaultsSetup
    {
        [FluentMember]
        private string name = string.Empty;

        [FluentMember]
        private int opacity = InitOpacity();

        private static int InitOpacity()
        {
            return -1;
        }
    }
}
