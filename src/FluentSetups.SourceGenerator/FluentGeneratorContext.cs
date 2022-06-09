// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentGeneratorContext.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
    using System;
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal class FluentGeneratorContext
    {
        #region Public Properties

        public ITypeSymbol BooleanType { get; private set; }

        public Compilation Compilation { get; private set; }

        public FluentRootInfo FluentRoot { get; set; }

        public INamedTypeSymbol VoidType { get; private set; }

        #endregion

        #region Properties

        internal static string FluentMemberAttributeName => "FluentSetups.FluentMemberAttribute";

        internal static string FluentRootAttributeName => "FluentSetups.FluentRootAttribute";

        internal static string FluentSetupAttributeName => "FluentSetups.FluentSetupAttribute";

        internal INamedTypeSymbol FluentMemberAttribute { get; private set; }

        internal INamedTypeSymbol FluentRootAttribute { get; private set; }

        internal INamedTypeSymbol FluentSetupAttribute { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static FluentGeneratorContext FromCompilation(Compilation compilation)
        {
            return new FluentGeneratorContext
            {
                Compilation = compilation,
                FluentRootAttribute = compilation.GetTypeByMetadataName(FluentRootAttributeName),
                FluentSetupAttribute = compilation.GetTypeByMetadataName(FluentSetupAttributeName),
                FluentMemberAttribute = compilation.GetTypeByMetadataName(FluentMemberAttributeName),
                BooleanType = compilation.GetTypeByMetadataName("System.Boolean"),
                VoidType = compilation.GetTypeByMetadataName("System.Void")
            };
        }

        public SetupClassInfo CreateFluentSetupInfo(ClassDeclarationSyntax setupCandidate)
        {
            if (InspectAndInitialize(setupCandidate, out SetupClassInfo classInfo))
                return classInfo;

            throw new ArgumentException($"The specified {nameof(ClassDeclarationSyntax)} is not a fluent setup class", nameof(setupCandidate));
        }

        public IEnumerable<SetupClassInfo> FindFluentSetups(IEnumerable<ClassDeclarationSyntax> setupCandidates)
        {
            foreach (var setupCandidate in setupCandidates)
            {
                if (InspectAndInitialize(setupCandidate, out SetupClassInfo classInfo))
                    yield return classInfo;
            }
        }

        public bool TryGetMissingType(out string missingType)
        {
            if (FluentSetupAttribute == null)
            {
                missingType = FluentSetupAttributeName;
                return true;
            }

            if (FluentMemberAttribute == null)
            {
                missingType = FluentMemberAttributeName;
                return true;
            }

            missingType = null;
            return false;
        }

        #endregion

        #region Methods

        internal bool IsFluentSetupAttribute(AttributeData attributeData)
        {
            if (FluentSetupAttribute.Equals(attributeData.AttributeClass, SymbolEqualityComparer.Default))
                return true;

            return false;
        }

        private bool InspectAndInitialize(ClassDeclarationSyntax candidate, out SetupClassInfo setupClassInfo)
        {
            setupClassInfo = null;
            var semanticModel = Compilation.GetSemanticModel(candidate.SyntaxTree);

            if (!(semanticModel.GetDeclaredSymbol(candidate) is INamedTypeSymbol classSymbol))
                return false;

            foreach (var attributeData in classSymbol.GetAttributes())
            {
                if (IsFluentSetupAttribute(attributeData))
                {
                    setupClassInfo = new SetupClassInfo(this, candidate, semanticModel, classSymbol, attributeData);
                    return true;
                }

                if (IsFluentRootAttribute(attributeData))
                {
                    FluentRoot = new FluentRootInfo(candidate, classSymbol);
                    return false;
                }
            }

            return false;
        }

        private bool IsFluentRootAttribute(AttributeData attributeData)
        {
            return FluentRootAttribute.Equals(attributeData.AttributeClass, SymbolEqualityComparer.Default);
        }

        private bool IsSetupClass(ClassDeclarationSyntax candidate)
        {
            var semanticModel = Compilation.GetSemanticModel(candidate.SyntaxTree);

            var classSymbol = (ITypeSymbol)semanticModel.GetDeclaredSymbol(candidate);

            if (classSymbol == null)
                return false;

            foreach (var attributeData in classSymbol.GetAttributes())
            {
                if (FluentSetupAttribute.Equals(attributeData.AttributeClass, SymbolEqualityComparer.Default))
                    return true;

                var attributeName = attributeData.AttributeClass?.Name;

                if (attributeName == "FluentSetupAttribute")
                    return true;

                if (attributeName == "FluentSetup")
                    return true;
            }

            return false;
        }

        #endregion

        public string GetFluentRootName(string defaultValue)
        {
            return FluentRoot?.ClassSymbol?.Name ?? defaultValue;
        }

        public string GetFluentRootNamespace(string defaultValue)
        {
            return FluentRoot?.ClassSymbol?.ContainingNamespace.ToString() ?? defaultValue;
        }
    }
}