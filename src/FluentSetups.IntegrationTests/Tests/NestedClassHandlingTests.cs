// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Class1.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using FluentAssertions;

using FluentSetups.IntegrationTests.Targets;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentSetups.IntegrationTests.Tests
{
    [TestClass]
    public class NestedClassHandlingTests
    {
        [TestMethod]
        public void EnsureDefaultFieldValuesWorkCorrectly()
        {
            var defaultObject = Setup.Person().Done();
            defaultObject.Age.Should().Be(10);
        }

        [FluentSetup(typeof(Person), SetupMethod = "NestedPerson")]
        internal partial class NestedSetup
        {
            [FluentMember]
            private int age = 10;
        }
    }
}
