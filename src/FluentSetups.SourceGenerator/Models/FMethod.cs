// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Linq;
   using System.Text;

   using Microsoft.CodeAnalysis;

   internal class FMethod : IFluentMember
   {
      #region Constants and Fields

      private readonly IMethodSymbol methodSymbol;

      #endregion

      #region Constructors and Destructors

      public FMethod(IMethodSymbol methodSymbol)
      {
         this.methodSymbol = methodSymbol ?? throw new ArgumentNullException(nameof(methodSymbol));
         MemberName = methodSymbol.Name;
         TypeName = methodSymbol.Parameters.FirstOrDefault()?.Type.ToString();
      }

      public string TypeName { get; }

      public FMethod(string methodName, ITypeSymbol argumentType, ITypeSymbol returnType)
      {
         MemberName = methodName;
         TypeName = argumentType.ToString();
         ReturnType = returnType.Name;
      }

      #endregion

      #region Public Properties

      public string MemberName { get; set; }

      #endregion

      public string ToCode()
      {
         var codeBuilder = new StringBuilder();
         codeBuilder.AppendLine($"private {ReturnType} {MemberName}({TypeName} value)");
         codeBuilder.AppendLine("{");
         codeBuilder.AppendLine("return this;");
         codeBuilder.AppendLine("}");

         return codeBuilder.ToString();

      }

      public bool IsUserDefined => methodSymbol != null;

      public string ReturnType { get; }
   }
}