// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupMethodBase.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;

   using Microsoft.CodeAnalysis;

   internal abstract class SetupMethodBase : MethodBase
   {
      #region Constructors and Destructors

      protected SetupMethodBase(FClass setupClass, string name, string parameterTypeName)
         : base(setupClass, name, parameterTypeName)
      {
         SetupClass = setupClass ?? throw new ArgumentNullException(nameof(setupClass));
      }

      protected SetupMethodBase(FClass setupClass, string name)
         : base(setupClass, name)
      {
         SetupClass = setupClass ?? throw new ArgumentNullException(nameof(setupClass));
      }

      protected SetupMethodBase(FClass setupClass, string name, ITypeSymbol parameterType)
         : base(setupClass, name, parameterType)
      {
         SetupClass = setupClass ?? throw new ArgumentNullException(nameof(setupClass));
      }

      #endregion

      #region Properties

      protected FClass SetupClass { get; }

      #endregion
   }
}