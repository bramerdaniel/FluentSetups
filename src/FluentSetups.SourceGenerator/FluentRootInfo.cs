// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentRootInfo.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal class FluentRootInfo
    {
        public ClassDeclarationSyntax ClassDeclarationSyntax { get; }

        public INamedTypeSymbol ClassSymbol { get; }

        public FluentRootInfo(ClassDeclarationSyntax classDeclarationSyntax, INamedTypeSymbol classSymbol)
        {
            ClassDeclarationSyntax = classDeclarationSyntax;
            ClassSymbol = classSymbol;
        }
    }
}