// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FGetValueOrDefaultMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Diagnostics;
   using System.Text;

   [DebuggerDisplay("{Signature}")]
   internal class FGetValueOrDefaultMethod : MethodBase
   {
      #region Constants and Fields

      private readonly IFluentTypedMember backingFieldSymbol;

      private readonly FField setupIndicatorField;

      #endregion

      #region Constructors and Destructors

      public FGetValueOrDefaultMethod(FClass setupClass, IFluentTypedMember backingFieldSymbol, FField setupIndicatorField)
      : base(setupClass, $"Get{backingFieldSymbol.Name.ToFirstUpper()}",$"System.Func<{backingFieldSymbol.Type}>")
      {
         this.backingFieldSymbol = backingFieldSymbol ?? throw new ArgumentNullException(nameof(backingFieldSymbol));
         this.setupIndicatorField = setupIndicatorField ?? throw new ArgumentNullException(nameof(setupIndicatorField));
      }

      #endregion

      #region IFluentMethod Members
      
      public override string ToCode()
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

      public override bool IsUserDefined => false;

      public override int ParameterCount => 1;

      #endregion
   }
}