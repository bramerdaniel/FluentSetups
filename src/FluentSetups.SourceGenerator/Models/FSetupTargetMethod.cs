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

   internal class FSetupTargetMethod : MethodBase
   {


      public FSetupTargetMethod(FClass setupClass)
         : base(setupClass, "SetupTarget", setupClass?.Target?.TypeSymbol)
      {
         ReturnTypeName = "void";
      }

      public override string ToCode()
      {
         var codeBuilder = new StringBuilder();
         codeBuilder.Append($"{ComputeModifier()} {ReturnTypeName} {Name}({ParameterTypeName} target)");
         codeBuilder.AppendLine("{");

         foreach (var setMethod in SetupClass.Methods.OfType<FSetupMemberMethod>())
            codeBuilder.AppendLine($"{setMethod.Name}(target);");
         
         codeBuilder.AppendLine("}");

         return codeBuilder.ToString();
      }

      public override bool IsUserDefined => false;

      public override int ParameterCount => 1;

      private string ComputeModifier()
      {
         if (SetupClass.Target.IsInternal && SetupClass.IsPublic)
            return "private";
         return "protected";
      }
   }
}