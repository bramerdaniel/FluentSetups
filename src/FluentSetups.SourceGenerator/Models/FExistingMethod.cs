// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FExistingMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Diagnostics;
   using System.Linq;

   using Microsoft.CodeAnalysis;

   [DebuggerDisplay("{Signature}")]
   internal class FExistingMethod : MethodBase
   {
      #region Constants and Fields

      private readonly IMethodSymbol methodSymbol;

      #endregion

      #region Constructors and Destructors

      public FExistingMethod(FClass setupClass, IMethodSymbol methodSymbol)
         : base(setupClass, methodSymbol?.Name, methodSymbol?.Parameters.FirstOrDefault()?.Type)
      {
         this.methodSymbol = methodSymbol ?? throw new ArgumentNullException(nameof(methodSymbol));
      }

      #endregion

      #region Public Properties

      public override bool IsUserDefined => true;

      public override int ParameterCount => methodSymbol.Parameters.Length;

      #endregion

      #region Properties

      /// <summary>Gets or sets the member that caused the generation of this method.</summary>
      internal IFluentMember Source { get; set; }

      #endregion

      #region Public Methods and Operators

      public override string ToCode()
      {
         // Existing methods are not generated !
         return "// NOT SUPPORTED";
      }

      #endregion
   }
}