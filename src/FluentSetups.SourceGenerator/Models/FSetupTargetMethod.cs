// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FSetupTargetMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Linq;
   using System.Text;

   using Microsoft.CodeAnalysis;

   internal class FSetupTargetMethod : FMethod
   {
      public FTarget Target { get; }

      public FSetupTargetMethod(IMethodSymbol methodSymbol, FTarget target)
         : base(methodSymbol)
      {
         Target = target ?? throw new ArgumentNullException(nameof(target));
      }

      public FSetupTargetMethod(FClass setupClass)
         : base("SetupTarget", setupClass.Target.TypeSymbol, null)
      {
      }

      public override string ToCode()
      {
         var codeBuilder = new StringBuilder();
         //codeBuilder.AppendLine($"/// <summary>");
         //codeBuilder.AppendLine($"// This method initializes the created <see cref=\"{ReturnType}\"/> instance");
         //codeBuilder.AppendLine($"///</summary>");
         codeBuilder.Append($"internal {ReturnType} {Name}({ParameterTypeName} target)");
         codeBuilder.AppendLine("{");
         codeBuilder.AppendLine($"   // TODO");
         codeBuilder.AppendLine("}");

         return codeBuilder.ToString();
      }
   }
}