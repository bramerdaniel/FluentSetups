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

      public FGetValueMethod(FClass setupClass, IFluentTypedMember backingFieldSymbol)
         : base(setupClass, $"Get{backingFieldSymbol?.Name?.ToFirstUpper()}")
      {
         this.backingFieldSymbol = backingFieldSymbol ?? throw new ArgumentNullException(nameof(backingFieldSymbol));
      }
      

      public override string ToCode()
      {
         var codeBuilder = new StringBuilder();
         codeBuilder.AppendLine($"protected {backingFieldSymbol.Type} {Name}()");
         codeBuilder.AppendLine("{");

         if (backingFieldSymbol.HasDefaultValue)
         {
            codeBuilder.AppendLine($"  return {backingFieldSymbol.Name};");
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