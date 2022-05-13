// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Diagnostics;
   using System.Linq;
   using System.Text;

   using Microsoft.CodeAnalysis;

   [DebuggerDisplay("{Name}")]
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
         Category = ComputeCategory(Name);
      }

      public FMethod(string methodName, ITypeSymbol parameterType, ITypeSymbol returnType)
      {
         Name = methodName ?? throw new ArgumentNullException(nameof(methodName));
         ParameterType = parameterType;
         ParameterTypeName = parameterType?.ToString();
         ReturnType = returnType?.Name ?? "void";
         ParameterCount = parameterType == null ? 0 : 1;
         Category = ComputeCategory(Name);
      }

      #endregion

      #region IFluentMethod Members

      public virtual string ToCode()
      {
         var codeBuilder = new StringBuilder();
         codeBuilder.Append($"internal {ReturnType} {Name}");
         codeBuilder.AppendLine(ParameterCount == 0 ? "()" : $"({ParameterTypeName} value)");

         codeBuilder.AppendLine("{");
         AppendMethodContent(codeBuilder);
         codeBuilder.AppendLine("}");

         return codeBuilder.ToString();
      }

      public bool IsUserDefined => methodSymbol != null;

      /// <summary>Gets the name of the method.</summary>
      public string Name { get; }

      public int ParameterCount { get; }

      public string Category { get; internal set; }

      #endregion

      #region Public Properties

      /// <summary>Gets the type of the first parameter parameter.</summary>
      public ITypeSymbol ParameterType { get; }

      public string ParameterTypeName { get; }

      public string ReturnType { get; }

      public FField SetupIndicatorField { get; set; }

      #endregion

      #region Properties

      /// <summary>Gets or sets the member that caused the generation of this method.</summary>
      internal IFluentMember Source { get; set; }

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

      protected virtual void AppendMethodContent(StringBuilder codeBuilder)
      {
         if (Source != null)
            codeBuilder.AppendLine($"   {Source.Name} = value;");
         if (SetupIndicatorField != null)
            codeBuilder.AppendLine($"   {SetupIndicatorField.Name} = true;");

         codeBuilder.AppendLine("   return this;");
      }

      protected bool Equals(FMethod other)
      {
         return Name == other.Name && ParameterCount == other.ParameterCount && ParameterTypeEquals(other);
      }

      private string ComputeCategory(string methodName)
      {
         if (methodName == "Done" || methodName == "CreateTarget" || methodName == "SetupTarget")
            return "TargetBuilder";

         return methodName;
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