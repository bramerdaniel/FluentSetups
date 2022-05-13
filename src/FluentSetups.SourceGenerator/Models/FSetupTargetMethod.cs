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
      public FClass SetupClass { get; }

      public FSetupTargetMethod(IMethodSymbol methodSymbol, FClass setupClass)
         : base(methodSymbol)
      {
         SetupClass = setupClass ?? throw new ArgumentNullException(nameof(setupClass));
      }

      public FSetupTargetMethod(FClass setupClass)
         : base("SetupTarget", setupClass?.Target?.TypeSymbol, null)
      {
         SetupClass = setupClass ?? throw new ArgumentNullException(nameof(setupClass));
      }

      public override string ToCode()
      {
         var codeBuilder = new StringBuilder();
         codeBuilder.Append($"{ComputeModifier()} {ReturnType} {Name}({ParameterTypeName} target)");
         codeBuilder.AppendLine("{");

         foreach (var setMethod in SetupClass.Methods.OfType<FSetupMemberMethod>())
            codeBuilder.AppendLine($"{setMethod.Name}(target);");
         
         codeBuilder.AppendLine("}");

         return codeBuilder.ToString();
      }

      private string ComputeModifier()
      {
         if (SetupClass.Target.IsInternal && SetupClass.IsPublic)
            return "private";
         return "protected";
      }
   }
}