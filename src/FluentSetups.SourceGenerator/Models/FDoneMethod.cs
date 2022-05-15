// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FDoneMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System.Diagnostics;
   using System.Text;

   [DebuggerDisplay("{Signature}")]
   internal class FDoneMethod : MethodBase
   {
      #region Constructors and Destructors

      public FDoneMethod(FClass setupClass)
         : base(setupClass, "Done", (string)null)
      {
         ReturnTypeName = setupClass.TargetTypeName;
      }

      #endregion

      #region Public Properties

      public override bool IsUserDefined => false;

      public override int ParameterCount => 0;

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

      protected void AppendMethodContent(StringBuilder codeBuilder)
      {
         codeBuilder.AppendLine("var target = CreateTarget();");
         codeBuilder.AppendLine("SetupTarget(target);");
         codeBuilder.AppendLine("return target;");
      }

      protected virtual string ComputeModifier()
      {
         return "internal";
      }

      #endregion
   }
}