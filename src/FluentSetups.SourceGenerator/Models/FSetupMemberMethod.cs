// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FSetupMemberMethod.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Text;

   internal class FSetupMemberMethod : SetupMethodBase
   {
      #region Constants and Fields

      private readonly IFluentTypedMember backingFieldSymbol;

      private readonly FField setupIndicatorField;

      #endregion

      #region Constructors and Destructors

      public FSetupMemberMethod(FClass setupClass, IFluentTypedMember backingFieldSymbol, FField setupIndicatorField)
         : base(setupClass, $"Setup{backingFieldSymbol?.Name?.ToFirstUpper()}", setupClass.Target.TypeSymbol)
      {
         this.backingFieldSymbol = backingFieldSymbol ?? throw new ArgumentNullException(nameof(backingFieldSymbol));
         this.setupIndicatorField = setupIndicatorField ?? throw new ArgumentNullException(nameof(setupIndicatorField));
      }

      #endregion

      #region Public Properties

      public override bool IsUserDefined => false;

      public override int ParameterCount => 1;

      #endregion

      #region Public Methods and Operators

      public override string ToCode()
      {
         var codeBuilder = new StringBuilder();
         codeBuilder.AppendLine($"{ComputeModifier()} void {Name}({SetupClass.TargetTypeName} target)");
         codeBuilder.AppendLine("{");
         GenerateContent(codeBuilder);
         codeBuilder.AppendLine("}");
         return codeBuilder.ToString();
      }

      #endregion

      #region Methods

      private string ComputeModifier()
      {
         if (SetupClass is FClass setupClass && setupClass.Target.IsInternal && setupClass.IsPublic)
            return "private";

         return "protected";
      }

      private void GenerateContent(StringBuilder codeBuilder)
      {
         if (!backingFieldSymbol.HasDefaultValue)
         {
            codeBuilder.AppendLine($"if (!{setupIndicatorField.Name})");
            codeBuilder.AppendLine("   return;");
            codeBuilder.AppendLine();
         }

         codeBuilder.AppendLine($"target.{backingFieldSymbol.Name.ToFirstUpper()} = {backingFieldSymbol.Name};");
      }

      #endregion
   }
}