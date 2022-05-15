// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FFluentSetupMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System.Diagnostics;
   using System.Text;

   using Microsoft.CodeAnalysis;

   [DebuggerDisplay("{Signature}")]
   internal class FFluentSetupMethod : MethodBase
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
            codeBuilder.AppendLine($"   {Source.Name} = value;");
         if (SetupIndicatorField != null)
            codeBuilder.AppendLine($"   {SetupIndicatorField.Name} = true;");

         codeBuilder.AppendLine("   return this;");
      }

      protected virtual string ComputeModifier()
      {
         return "internal";
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