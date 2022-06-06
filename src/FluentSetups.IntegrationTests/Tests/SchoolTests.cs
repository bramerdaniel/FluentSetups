// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SchoolTests.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

using FluentAssertions;

using FluentSetups.IntegrationTests.Targets;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentSetups.IntegrationTests.Tests
{
    [TestClass]
    public class ListSetupTests
    {
        [TestMethod]
        public void SetupSchool()
        {
            var school = Setup.School()
                .WithChildren(new List<Child>
                {
                    new() { Name = "Rob" },
                    new() { Name = "Bob" }
                })
                .Done();

            school.Children.Should().HaveCount(2);
        }

        [TestMethod]
        public void SetupSchoolWithSingleChildren()
        {
            var school = Setup.School()
                .WithChild(new Child{ Name = "Rob" })
                .WithChild(new Child{ Name = "Bob" })
                .WithChild(new Child{ Name = "John" })
                .Done();

            school.Children.Should().HaveCount(2);

        }
    }

}
