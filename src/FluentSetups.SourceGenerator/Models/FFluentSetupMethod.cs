﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FFluentSetupMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Diagnostics;
   using System.Text;

   using Microsoft.CodeAnalysis;

   [DebuggerDisplay("{Signature}")]
   internal class FFluentSetupMethod : SetupMethodBase
   {
      #region Constructors and Destructors

      public FFluentSetupMethod(FClass setupClass, string methodName, ITypeSymbol parameterType, ITypeSymbol returnType)
         : base(setupClass, methodName, parameterType)
      {
         ReturnTypeName = returnType?.Name ?? "void";
         ParameterCount = parameterType == null ? 0 : 1;
         Category = ComputeCategory(Name);
      }

      #endregion

      #region Public Properties

      public override bool IsUserDefined => false;

      public override int ParameterCount { get; }

      public FField SetupIndicatorField { get; set; }

      #endregion

      #region Properties

      /// <summary>Gets or sets the member that caused the generation of this method.</summary>
      internal IFluentMember Source { get; set; }

      #endregion

      #region Public Methods and Operators

      public override string ToCode()
      {
         var codeBuilder = new StringBuilder();
         codeBuilder.Append($"{ComputeModifier()} {ReturnTypeName} {Name}");
         codeBuilder.AppendLine(ParameterCount == 0 ? "()" : $"({ParameterTypeName} value)");

         codeBuilder.AppendLine("{");
         AppendMethodContent(codeBuilder);
         codeBuilder.AppendLine("}");

         return codeBuilder.ToString();
      }

      #endregion

      #region Methods

      protected virtual void AppendMethodContent(StringBuilder codeBuilder)
      {
         if (Source != null)
            codeBuilder.AppendLine($"   {ComputeSourceName()} = value;");
         if (SetupIndicatorField != null)
            codeBuilder.AppendLine($"   {SetupIndicatorField.Name} = true;");

         codeBuilder.AppendLine("   return this;");

         string ComputeSourceName()
         {
            return string.Equals(Source.Name, "value", StringComparison.InvariantCulture) 
               ? $"this.{Source.Name}"
               : Source.Name;
         }
      }

      protected virtual string ComputeModifier()
      {
         return SetupClass.Modifier;
      }

      private string ComputeCategory(string methodName)
      {
         if (methodName == "Done" || methodName == "CreateTarget" || methodName == "SetupTarget")
            return "TargetBuilder";

         return methodName;
      }

      #endregion
   }
}