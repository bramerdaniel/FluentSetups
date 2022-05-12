// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FCreateTarget.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Linq;
   using System.Text;

   using Microsoft.CodeAnalysis;

   internal class FCreateTarget : FMethod
   {
      public FTarget Target { get; }

      public FCreateTarget(IMethodSymbol methodSymbol, FTarget target)
         : base(methodSymbol)
      {
         Target = target ?? throw new ArgumentNullException(nameof(target));
      }

      public FCreateTarget(ITypeSymbol returnType, FTarget target)
         : base("CreateTarget", null, target?.TypeSymbol)
      {
         Target = target ?? throw new ArgumentNullException(nameof(target));
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