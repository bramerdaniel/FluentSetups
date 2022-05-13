// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FDoneMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Text;

   using Microsoft.CodeAnalysis;

   internal class FDoneMethod : FMethod
   {
      public FDoneMethod(IMethodSymbol methodSymbol)
         : base(methodSymbol)
      {
      }

      public FDoneMethod(ITypeSymbol returnType)
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