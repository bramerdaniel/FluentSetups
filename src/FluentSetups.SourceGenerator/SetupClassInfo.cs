﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupClassInfo.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using System;
   using System.Linq;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp.Syntax;

   /// <summary>Data class containing all the required information along the generation process</summary>
   internal class SetupClassInfo
   {
      #region Constructors and Destructors

      public SetupClassInfo(FluentGeneratorContext context, ClassDeclarationSyntax candidate, SemanticModel semanticModel, ITypeSymbol classSymbol,
         AttributeData fluentSetupAttribute)
      {
         Context = context;
         ClassSyntax = candidate ?? throw new ArgumentNullException(nameof(candidate));
         SemanticModel = semanticModel ?? throw new ArgumentNullException(nameof(semanticModel));
         ClassSymbol = classSymbol ?? throw new ArgumentNullException(nameof(classSymbol));
         FluentSetupAttribute = fluentSetupAttribute ?? throw new ArgumentNullException(nameof(fluentSetupAttribute));
      }

      #endregion

      #region Public Properties

      /// <summary>Gets or sets the class symbol.</summary>
      public ITypeSymbol ClassSymbol { get; }

      public ClassDeclarationSyntax ClassSyntax { get; }

      public AttributeData FluentSetupAttribute { get; }

      public SemanticModel SemanticModel { get; }

      public TypedConstant TargetMode => FluentSetupAttribute.GetTargetMode();

      public TypedConstant TargetType => FluentSetupAttribute.GetTargetType();

      #endregion

      #region Properties

      private FluentGeneratorContext Context { get; }

      #endregion

      #region Public Methods and Operators

      public bool IsValidSetup()
      {
         if (ClassSymbol == null)
            return false;

         if (FluentSetupAttribute == null)
            return false;

         return true;
      }

      #endregion

      #region Methods

      internal string GetSetupEntryClassName()
      {
         if (TryGetConstructorArgument(TypedConstantKind.Primitive, out var targetType))
            return targetType.Value.ToString();

         if (TryGetNamedArgument("EntryClassName", out targetType) && targetType.Kind == TypedConstantKind.Primitive)
            return targetType.Value.ToString();

         // TODO return default atttibute value
         return "Setup";
      }

      internal string GetSetupEntryNameSpace()
      {
         if (TryGetNamedArgument("EntryNamespace", out var targetType) && targetType.Kind == TypedConstantKind.Primitive)
            return targetType.Value?.ToString();

         // This should be the default namespace of the containing assembly
         return ClassSymbol.ContainingAssembly.MetadataName;
      }

      private TypedConstant GetTargetMode()
      {
         if (TryGetNamedArgument("TargetMode", out var targetType) && targetType.Kind == TypedConstantKind.Enum)
            return targetType;
         return default;
      }

      private TypedConstant GetTargetType()
      {
         if (TryGetConstructorArgument(TypedConstantKind.Type, out var targetType))
            return targetType;
         if (TryGetNamedArgument("TargetType", out targetType) && targetType.Kind == TypedConstantKind.Type)
            return targetType;
         return default;
      }

      private bool IsFluentSetupAttribute(AttributeData attributeData)
      {
         return IsFluentSetupAttribute(attributeData.AttributeClass);
      }

      private bool IsFluentSetupAttribute(INamedTypeSymbol attributeSymbol)
      {
         return Context.FluentSetupAttribute.Equals(attributeSymbol, SymbolEqualityComparer.Default);
      }

      private bool TryGetConstructorArgument(TypedConstantKind type, out TypedConstant targetType)
      {
         var attribute = FluentSetupAttribute;
         if (attribute != null && attribute.ConstructorArguments.Length > 0)
         {
            targetType = attribute.ConstructorArguments.FirstOrDefault(x => x.Kind == type);
            return !targetType.IsNull;
         }

         targetType = default;
         return false;
      }

      private bool TryGetNamedArgument(string argumentName, out TypedConstant typedConstant)
      {
         var attribute = FluentSetupAttribute;
         if (attribute != null && attribute.NamedArguments.Length > 0)
         {
            var match = attribute.NamedArguments.FirstOrDefault(x => x.Key == argumentName);
            if (match.Key != null)
            {
               typedConstant = match.Value;
               return true;
            }
         }

         typedConstant = default;
         return false;
      }

      #endregion
   }
}