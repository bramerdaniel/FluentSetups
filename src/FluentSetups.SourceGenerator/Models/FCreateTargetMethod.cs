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
   internal class FCreateTargetMethod : FMethod
   {
      public FClass SetupClass { get; }

      public FTarget Target { get; }

      public FCreateTargetMethod(IMethodSymbol methodSymbol, FTarget target)
         : base(methodSymbol)
      {
         Target = target ?? throw new ArgumentNullException(nameof(target));
      }

      public FCreateTargetMethod(FClass setupClass)
         : base("CreateTarget", null, setupClass?.Target?.TypeSymbol)
      {
         SetupClass = setupClass ?? throw new ArgumentNullException(nameof(setupClass));
         Target = setupClass.Target;
      }

      protected override string ComputeModifier()
      {
         if (Target.IsInternal && SetupClass.IsPublic)
            return "private";
         return "protected";
      }

      protected override void AppendMethodContent(StringBuilder codeBuilder)
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