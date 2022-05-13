// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FGetValueMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Text;

   internal class FGetValueMethod : IFluentMethod
   {
      private readonly IFluentTypedMember backingFieldSymbol;

      public FGetValueMethod(IFluentTypedMember backingFieldSymbol)
      {
         this.backingFieldSymbol = backingFieldSymbol ?? throw new ArgumentNullException(nameof(backingFieldSymbol));
         Name = $"Get{backingFieldSymbol.Name.ToFirstUpper()}";
      }

      public string Name { get; }

      public string ToCode()
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

      public bool IsUserDefined => false;

      public int ParameterCount => 0;

      public string Category { get; set; }
   }
}