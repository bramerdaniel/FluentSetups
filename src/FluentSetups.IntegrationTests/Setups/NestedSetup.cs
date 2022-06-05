// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NestedSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Setups
{
    public class Animal
    {
        #region Public Properties

        public string Name { get; set; }

        #endregion
    }

    public class OuterClass
    {
        [FluentSetup(typeof(Animal))]
        public partial class AnimalSetup
        {
        }
    }
}
