// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FSetupMemberMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Text;

   internal class FSetupMemberMethod : FMethod
   {
      #region Constants and Fields

      private readonly IFluentTypedMember backingFieldSymbol;

      private readonly FField setupIndicatorField;

      #endregion

      #region Constructors and Destructors

      public FSetupMemberMethod(FClass setupClass, IFluentTypedMember backingFieldSymbol, FField setupIndicatorField)
         : base(setupClass, $"Setup{backingFieldSymbol?.Name?.ToFirstUpper()}", setupClass.Target.TypeSymbol ,null)
      {
         this.backingFieldSymbol = backingFieldSymbol ?? throw new ArgumentNullException(nameof(backingFieldSymbol));
         this.setupIndicatorField = setupIndicatorField ?? throw new ArgumentNullException(nameof(setupIndicatorField));
      }

      #endregion

      #region IFluentMethod Members
      
      public override string ToCode()
      {
         var codeBuilder = new StringBuilder();
         codeBuilder.AppendLine($"{ComputeModifier()} void {Name}({SetupClass.TargetTypeName} target)");
         codeBuilder.AppendLine("{");
         codeBuilder.AppendLine($"if (!{setupIndicatorField.Name})");
         codeBuilder.AppendLine("   return;");
         codeBuilder.AppendLine();
         codeBuilder.AppendLine($"target.{backingFieldSymbol.Name.ToFirstUpper()} = {backingFieldSymbol.Name};");
         codeBuilder.AppendLine("}");
         return codeBuilder.ToString();
      }

      public override bool IsUserDefined => false;

      public override int ParameterCount => 1;
      

      #endregion
      
      #region Methods

      protected override string ComputeModifier()
      {
         if (SetupClass.Target.IsInternal && SetupClass.IsPublic)
            return "private";
         return "protected";
      }

      #endregion
   }
}