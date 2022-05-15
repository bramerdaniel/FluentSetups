// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Diagnostics;
   using System.Linq;
   using System.Text;

   using Microsoft.CodeAnalysis;

   [DebuggerDisplay("{Signature}")]
   internal class FMethod : MethodBase
   {
      #region Constants and Fields

      private readonly IMethodSymbol methodSymbol;

      #endregion

      #region Constructors and Destructors

      public FMethod(FClass setupClass, IMethodSymbol methodSymbol)
      : base(setupClass, methodSymbol.Name , methodSymbol.Parameters.FirstOrDefault()?.Type)
      {
         this.methodSymbol = methodSymbol ?? throw new ArgumentNullException(nameof(methodSymbol));
         ParameterCount = methodSymbol.Parameters.Length;
         Category = ComputeCategory(Name);
      }

      public FMethod(FClass setupClass, string methodName, ITypeSymbol parameterType, ITypeSymbol returnType)
      : base(setupClass, methodName, parameterType)
      {
         ReturnTypeName = returnType?.Name ?? "void";
         ParameterCount = parameterType == null ? 0 : 1;
         Category = ComputeCategory(Name);
      }

      #endregion

      #region IFluentMethod Members

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

      protected virtual string ComputeModifier()
      {
         return "internal";
      }

      public override bool IsUserDefined => methodSymbol != null;

      public override int ParameterCount { get; }
      

      #endregion

      #region Public Properties


      public FField SetupIndicatorField { get; set; }

      #endregion

      #region Properties

      /// <summary>Gets or sets the member that caused the generation of this method.</summary>
      internal IFluentMember Source { get; set; }

      #endregion

      #region Public Methods and Operators



      #endregion

      #region Methods

      protected virtual void AppendMethodContent(StringBuilder codeBuilder)
      {
         if (Source != null)
            codeBuilder.AppendLine($"   {Source.Name} = value;");
         if (SetupIndicatorField != null)
            codeBuilder.AppendLine($"   {SetupIndicatorField.Name} = true;");

         codeBuilder.AppendLine("   return this;");
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