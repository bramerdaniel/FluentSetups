// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FSetupTargetMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System.Linq;
   using System.Text;

   internal class FSetupTargetMethod : MethodBase
   {
      #region Constructors and Destructors

      public FSetupTargetMethod(FClass setupClass)
         : base(setupClass, "SetupTarget", setupClass?.Target?.TypeSymbol)
      {
         ReturnTypeName = "void";
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
         codeBuilder.Append($"{ComputeModifier()} {ReturnTypeName} {Name}({ParameterTypeName} target)");
         codeBuilder.AppendLine("{");

         foreach (var setMethod in SetupClass.Methods.OfType<FSetupMemberMethod>())
            codeBuilder.AppendLine($"{setMethod.Name}(target);");

         codeBuilder.AppendLine("}");

         return codeBuilder.ToString();
      }

      #endregion

      #region Methods

      private string ComputeModifier()
      {
         if (SetupClass.Target.IsInternal && SetupClass.IsPublic)
            return "private";
         return "protected";
      }

      #endregion
   }
}