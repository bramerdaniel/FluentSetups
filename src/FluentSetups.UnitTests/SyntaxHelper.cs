// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyntaxHelper.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests;

using System.Collections.Generic;

using FluentSetups.SourceGenerator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SyntaxHelper : CSharpSyntaxWalker
{
   private readonly FluentSetupSyntaxReceiver receiver;

   public SyntaxHelper()
   {
      receiver = new FluentSetupSyntaxReceiver();
   }

   public override void VisitClassDeclaration(ClassDeclarationSyntax node)
   {
      receiver.OnVisitSyntaxNode(node);
      base.VisitClassDeclaration(node);
   }

   internal IList<ClassDeclarationSyntax> GetSetupClasses()
   {
      return receiver.SetupCandidates;
   }

   internal static IEnumerable<ClassDeclarationSyntax> FindSetupClasses(SyntaxTree syntaxTree)
   {
      var syntaxWalker = new SyntaxHelper();
      syntaxWalker.Visit(syntaxTree.GetRoot());
      return syntaxWalker.GetSetupClasses();
   }
}