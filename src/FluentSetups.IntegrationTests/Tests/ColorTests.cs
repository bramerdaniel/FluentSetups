// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentSetups.IntegrationTests.Tests
{
    [TestClass]
    public class ColorTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void SetupColorWithDefaultName()
        {
            var color = Setup.ColorWithDefaults().Done();

            color.Name.Should().Be(string.Empty);
            color.Opacity.Should().Be(-1);
        }

        #endregion
    }
}
