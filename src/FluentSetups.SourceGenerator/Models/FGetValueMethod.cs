// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FGetValueMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Diagnostics;
   using System.Text;
   
   [DebuggerDisplay("{Signature}")]
   internal class FGetValueMethod : MethodBase
   {
      private readonly IFluentTypedMember backingFieldSymbol;

      public FGetValueMethod(IFluentTypedMember backingFieldSymbol)
         : base($"Get{backingFieldSymbol?.Name?.ToFirstUpper()}")
      {
         this.backingFieldSymbol = backingFieldSymbol ?? throw new ArgumentNullException(nameof(backingFieldSymbol));
      }
      

      public override string ToCode()
      {
         var codeBuilder = new StringBuilder();
         codeBuilder.AppendLine($"protected {backingFieldSymbol.Type} {Name}()");
         codeBuilder.AppendLine("{");
         if (backingFieldSymbol is FField field && field.DefaultValue != null)
         {

            codeBuilder.AppendLine($"  return {Name}(() => {field.DefaultValue});");
         }
         else
         {
            codeBuilder.AppendLine($"  return {Name}(() => throw new SetupMemberNotInitializedException(nameof({backingFieldSymbol.Name})));");
         }

         codeBuilder.AppendLine("}");
         return codeBuilder.ToString();
      }

      public override bool IsUserDefined => false;

      public override int ParameterCount => 0;
   }
}