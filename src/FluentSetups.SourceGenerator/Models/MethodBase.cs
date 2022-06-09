// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodBase.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Text;

   using Microsoft.CodeAnalysis;

   internal abstract class MethodBase : IFluentMethod
   {
      #region Constants and Fields

      private string signature;

      #endregion

      #region Constructors and Destructors

      /// <summary>Initializes a new instance of the <see cref="MethodBase"/> class.</summary>
      /// <param name="setupClass">The setup class this method belongs to.</param>
      /// <param name="name">The name of the method.</param>
      /// <param name="parameterTypeName">Name of the parameter type of the first parameter.</param>
      /// <exception cref="System.ArgumentNullException">name</exception>
      protected MethodBase(IFluentClass setupClass, string name, string parameterTypeName)
      {
         ContainingClass = setupClass ?? throw new ArgumentNullException(nameof(setupClass));
         Name = name ?? throw new ArgumentNullException(nameof(name));
         ParameterTypeName = parameterTypeName;
      }

      protected MethodBase(IFluentClass setupClass, string name)
         : this(setupClass, name, (string)null)
      {
      }

      protected MethodBase(IFluentClass setupClass, string name, ITypeSymbol parameterType)
         : this(setupClass, name, parameterType?.ToString())
      {
         Name = name ?? throw new ArgumentNullException(nameof(name));
         ParameterType = parameterType;
      }

      #endregion

      #region IFluentMethod Members

      public string Name { get; }

      public abstract string ToCode();

      public abstract bool IsUserDefined { get; }

      public abstract int ParameterCount { get; }

      public string Category { get; set; }

      /// <summary>Gets the type of the first parameter parameter.</summary>
      public ITypeSymbol ParameterType { get; }

      #endregion

      #region Public Properties

      public string ParameterTypeName { get; }

      public string Documentation { get; set; }

      public string ReturnTypeName { get; protected set; } = "void";

      /// <summary>Gets the signature of the method.</summary>
      public string Signature => signature ?? (signature = ComputeSignature());

      #endregion

      #region Properties

      protected IFluentClass ContainingClass { get; }

      #endregion

      #region Public Methods and Operators

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj))
            return false;
         if (ReferenceEquals(this, obj))
            return true;
         if (obj is MethodBase otherMethod)
            return Equals(otherMethod);
         return false;
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

      protected bool Equals(MethodBase other)
      {
         return Signature.Equals(other.Signature);
      }

      private string ComputeSignature()
      {
         var builder = new StringBuilder(Name);
         builder.Append("(");

         if (ParameterTypeName != null)
            builder.Append(ParameterTypeName);

         builder.Append(")");
         return builder.ToString();
      }

      protected void AppendDocumentation(StringBuilder codeBuilder)
      {
         if(string.IsNullOrWhiteSpace(Documentation))
            return;

         codeBuilder.AppendLine($"/// <summary>{Documentation}</summary>");
      }

      #endregion
   }
}