// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FWithListElementMethod.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Text;

   internal class FWithListElementMethod : MethodBase
   {
      #region Constants and Fields

      private readonly IFluentTypedMember backingFieldListSymbol;

      #endregion

      #region Constructors and Destructors

      public FWithListElementMethod(FClass setupClass, IFluentTypedMember backingFieldSymbol)
         : base(setupClass, ComputeWithElementName(backingFieldSymbol), setupClass.Target.TypeSymbol)
      {
         backingFieldListSymbol = backingFieldSymbol ?? throw new ArgumentNullException(nameof(backingFieldSymbol));
         
         if (!backingFieldSymbol.IsListMember)
            throw new ArgumentException("Only list members are supported");
      }

      private static string ComputeWithElementName(IFluentTypedMember backingFieldSymbol)
      {
         var fieldName = backingFieldSymbol.Name.ToFirstUpper();
         if (fieldName.EndsWith("s"))
            return $"With{fieldName.Substring(0, fieldName.Length - 1)}";

         switch (fieldName)
         {
            case "Children":
               return "WithChild";
            case "People":
               return "WithPerson";
            default:
               return $"With{fieldName}";
         }
      }

      #endregion

      #region IFluentMethod Members

      public override string ToCode()
      {
         var codeBuilder = new StringBuilder();
         codeBuilder.AppendLine($"{SetupClass.Modifier} {SetupClass.ClassSymbol.Name} {Name}({backingFieldListSymbol.ElementType} value)");
         codeBuilder.AppendLine("{");
         GenerateContent(codeBuilder);
         codeBuilder.AppendLine("}");
         return codeBuilder.ToString();
      }


      private void GenerateContent(StringBuilder codeBuilder)
      {
         var listName = backingFieldListSymbol.Name;
         codeBuilder.AppendLine($"if({listName} == null)");
         codeBuilder.AppendLine($"   {listName} = new List<{backingFieldListSymbol.ElementType}>();");
         codeBuilder.AppendLine($"{listName}.Add(value);");
         codeBuilder.AppendLine($"{listName}WasSet = true;");
         codeBuilder.AppendLine("return this;");
      }

      public override bool IsUserDefined => false;

      public override int ParameterCount => 1;


      #endregion

      #region Methods

      #endregion
   }
}