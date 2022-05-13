// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FDone.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System.Text;

   using Microsoft.CodeAnalysis;

   internal class FDone : FMethod
   {
      public FDone(IMethodSymbol methodSymbol)
         : base(methodSymbol)
      {
      }

      public FDone(ITypeSymbol returnType)
         : base("Done", null, returnType)
      {
      }

      protected override void AppendMethodContent(StringBuilder codeBuilder)
      {
         codeBuilder.AppendLine("var target = CreateTarget();");
         codeBuilder.AppendLine("SetupTarget(target);");
         codeBuilder.AppendLine("return target;");
      }
   }
}