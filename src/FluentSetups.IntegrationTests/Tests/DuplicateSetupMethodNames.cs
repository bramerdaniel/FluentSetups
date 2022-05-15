// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DuplicateSetupMethodNames.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.IntegrationTests.Tests;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class DuplicateSetupMethodNames
{
   [TestMethod]
   public void EnsureOverwrittenDoneCompiles()
   {
      Setup.Olga().Done().LastName.Should().Be("OlgaSetup");
      Setup.Olga1().Done().LastName.Should().Be("AnotherOlgaSetup");
   }
}