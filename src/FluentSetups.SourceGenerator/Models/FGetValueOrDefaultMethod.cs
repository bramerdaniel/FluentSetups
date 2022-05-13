// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FGetValueOrDefaultMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Text;

   internal class FGetValueOrDefaultMethod : IFluentMethod
   {
      #region Constants and Fields

      private readonly IFluentTypedMember backingFieldSymbol;

      private readonly FField setupIndicatorField;

      #endregion

      #region Constructors and Destructors

      public FGetValueOrDefaultMethod(IFluentTypedMember backingFieldSymbol, FField setupIndicatorField)
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
         AppendContent(codeBuilder);
         codeBuilder.AppendLine("}");
         return codeBuilder.ToString();
      }

      private void AppendContent(StringBuilder codeBuilder)
      {
         codeBuilder.AppendLine($"if ({setupIndicatorField.Name})");
         codeBuilder.AppendLine($"   return {backingFieldSymbol.Name};");
         codeBuilder.AppendLine();
         codeBuilder.AppendLine(
            $"return defaultValue != null ? defaultValue() : throw new SetupMemberNotInitializedException(nameof({backingFieldSymbol.Name}));");
      }

      public bool IsUserDefined => false;

      public int ParameterCount => 1;

      public string Category { get; set; }

      #endregion
   }
}