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
      #region Constants and Fields

      private readonly IFluentTypedMember backingFieldSymbol;

      private readonly FField setupIndicatorField;

      #endregion

      #region Constructors and Destructors

      public FGetValueMethod(IFluentTypedMember backingFieldSymbol, FField setupIndicatorField)
      {
         this.backingFieldSymbol = backingFieldSymbol ?? throw new ArgumentNullException(nameof(backingFieldSymbol));
         this.setupIndicatorField = setupIndicatorField ?? throw new ArgumentNullException(nameof(setupIndicatorField));

         Name = $"Get{backingFieldSymbol.Name.ToFirstUpper()}";
      }

      #endregion

      #region IFluentMethod Members

      public string Name { get; }

      public string ToCode()
      {
         var codeBuilder = new StringBuilder();
         codeBuilder.AppendLine($"protected {backingFieldSymbol.Type} {Name}(Func<{backingFieldSymbol.Type}> defaultValue)");
         codeBuilder.AppendLine("{");
         codeBuilder.AppendLine($"  return {setupIndicatorField.Name} ? {backingFieldSymbol.Name} : defaultValue();");
         codeBuilder.AppendLine("}");
         return codeBuilder.ToString();
      }

      public bool IsUserDefined => false;

      public int ParameterCount => 1;

      public string Category { get; set; }

      #endregion
   }
}