// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FConstructorParameter.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;

   using Microsoft.CodeAnalysis;

   public class FConstructorParameter : IFluentTypedMember
   {
      #region Constants and Fields

      private readonly IParameterSymbol parameterSymbol;

      #endregion

      #region Constructors and Destructors

      public FConstructorParameter(IParameterSymbol parameterSymbol)
      {
         this.parameterSymbol = parameterSymbol ?? throw new ArgumentNullException(nameof(parameterSymbol));
      }

      #endregion

      #region IFluentTypedMember Members

      public bool IsUserDefined => false;

      public string Name => parameterSymbol.Name;

      public ITypeSymbol Type => parameterSymbol.Type;

      // TODO handel this
      public bool HasDefaultValue { get; } = false;

      public bool IsListMember { get; }

      public ITypeSymbol ElementType { get; }

      public string ToCode()
      {
         return "// FConstructorParameter does not support code generation";
      }

      #endregion
   }
}