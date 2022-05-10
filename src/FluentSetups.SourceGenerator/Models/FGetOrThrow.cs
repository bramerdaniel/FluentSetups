// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FGetOrThrow.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Text;

   internal class FGetOrThrow : IFluentMethod
   {
      private readonly IFluentTypedMember backingFieldSymbol;

      private readonly FField setupIndicatorField;

      public FGetOrThrow(IFluentTypedMember backingFieldSymbol, FField setupIndicatorField)
      {
         this.backingFieldSymbol = backingFieldSymbol ?? throw new ArgumentNullException(nameof(backingFieldSymbol));
         this.setupIndicatorField = setupIndicatorField ?? throw new ArgumentNullException(nameof(setupIndicatorField));
         
         Name = $"Get{backingFieldSymbol.Name.ToFirstUpper()}OrThrow";
      }

      public string Name { get; }

      public string ToCode()
      {
         var codeBuilder = new StringBuilder();
         codeBuilder.AppendLine($"protected {backingFieldSymbol.Type} {Name}()");
         codeBuilder.AppendLine("{");
         codeBuilder.AppendLine($"  return {setupIndicatorField.Name} ? {backingFieldSymbol.Name} : throw new InvalidOperationException(\"The member {backingFieldSymbol.Name} was not initialized.\");");
         codeBuilder.AppendLine("}");
         return codeBuilder.ToString();
      }

      public bool IsUserDefined => false;

      public int ParameterCount => 0;
   }
}