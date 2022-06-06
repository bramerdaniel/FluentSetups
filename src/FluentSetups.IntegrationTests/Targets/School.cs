// --------------------------------------------------------------------------------------------------------------------
// <copyright file="School.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace FluentSetups.IntegrationTests.Targets
{
    public class School
    {
        #region Public Properties

        public IList<Child> Children { get; set; }

        #endregion
    }

    public class Child
    {
        #region Public Properties

        public string Name { get; set; }

        #endregion
    }
}
