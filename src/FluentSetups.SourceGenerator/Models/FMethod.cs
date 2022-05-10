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

   internal class FMethod : IFluentMethod
   {
      #region Constants and Fields

      private readonly IMethodSymbol methodSymbol;

      #endregion

      #region Constructors and Destructors

      public FMethod(IMethodSymbol methodSymbol)
      {
         this.methodSymbol = methodSymbol ?? throw new ArgumentNullException(nameof(methodSymbol));
         Name = methodSymbol.Name;
         ParameterType = methodSymbol.Parameters.FirstOrDefault()?.Type;
         ParameterTypeName = ParameterType?.ToString();
         ParameterCount = methodSymbol.Parameters.Length;
      }

      public FMethod(string methodName, ITypeSymbol parameterType, ITypeSymbol returnType)
      {
         Name = methodName ?? throw new ArgumentNullException(nameof(methodName));
         ParameterType = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
         ParameterTypeName = parameterType.ToString();
         ReturnType = returnType.Name;
         ParameterCount = 1;
      }

      #endregion

      #region IFluentMember Members

      /// <summary>Gets or sets the member that caused the generation of this method.</summary>
      internal IFluentMember Source { get; set; }

      public string ToCode()
      {
         var codeBuilder = new StringBuilder();
         codeBuilder.AppendLine($"internal {ReturnType} {Name}({ParameterTypeName} value)");
         codeBuilder.AppendLine("{");
         AppendMethodContent(codeBuilder);
         codeBuilder.AppendLine("}");

         return codeBuilder.ToString();
      }

      private void AppendMethodContent(StringBuilder codeBuilder)
      {
         if (Source != null)
            codeBuilder.AppendLine($"   {Source.Name} = value;");
         if (SetupIndicatorField != null)
            codeBuilder.AppendLine($"   {SetupIndicatorField.Name} = true;");
         
         codeBuilder.AppendLine("   return this;");
      }

      public bool IsUserDefined => methodSymbol != null;

      #endregion

      #region Public Properties

      /// <summary>Gets the name of the method.</summary>
      public string Name { get; }

      public int ParameterCount { get; }

      /// <summary>Gets the type of the first parameter parameter.</summary>
      public ITypeSymbol ParameterType { get; }

      public string ReturnType { get; }

      public string ParameterTypeName { get; }

      public FField SetupIndicatorField { get; set; }

      #endregion

      #region Public Methods and Operators

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj))
            return false;
         if (ReferenceEquals(this, obj))
            return true;
         if (obj.GetType() != this.GetType())
            return false;
         return Equals((FMethod)obj);
      }

      public override int GetHashCode()
      {
         unchecked
         {
            var hashCode = ParameterType != null ? SymbolEqualityComparer.Default.GetHashCode(ParameterType) : 0;
            hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ ParameterCount;
            return hashCode;
         }
      }

      #endregion

      #region Methods

      protected bool Equals(FMethod other)
      {
         return Name == other.Name && ParameterCount == other.ParameterCount && ParameterTypeEquals(other);
      }

      private bool ParameterTypeEquals(FMethod other)
      {
         if (ParameterType == null && other.ParameterType == null)
            return true;

         if (ParameterType != null && other.ParameterType == null)
            return false;

         return ParameterType.Equals(other.ParameterType, SymbolEqualityComparer.Default);
      }

      #endregion
   }
}