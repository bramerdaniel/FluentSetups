// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FCreateTargetMethod.cs" company="consolovers">
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
   internal class FCreateTargetMethod : MethodBase
   {
      public FTarget Target { get; }


      public FCreateTargetMethod(FClass setupClass)
         : base(setupClass, "CreateTarget", (string)null)
      {
         ReturnTypeName = setupClass.TargetTypeName;
         Target = setupClass.Target;
      }

      protected string ComputeModifier()
      {
         if (Target.IsInternal && SetupClass.IsPublic)
            return "private";
         return "protected";
      }

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

      public override bool IsUserDefined => false;

      public override int ParameterCount => 0;

      protected void AppendMethodContent(StringBuilder codeBuilder)
      {
         codeBuilder.AppendLine($"   var target = {CreateConstructorCall()};");
         codeBuilder.AppendLine("   return target;");
      }

      private string CreateConstructorCall()
      {
         var arguments = string.Join(", ", Target.ConstructorParameters.Select(p => $"Get{p.Name.ToFirstUpper()}(null)"));
         var builder = new StringBuilder($"new {Target.TypeName}({arguments})");
         return builder.ToString();
      }
   }
}