// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentSetupSyntaxReceiver.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using System.Collections.Generic;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp.Syntax;

   /// <summary>
   /// Created on demand before each generation pass
   /// </summary>
   internal class FluentSetupSyntaxReceiver : ISyntaxReceiver
   {
      public List<ClassDeclarationSyntax> SetupCandidates { get; } = new List<ClassDeclarationSyntax>();

      /// <summary>
      /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
      /// </summary>
      public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
      {
         // any field with at least one attribute is a candidate for property generation
         if (!(syntaxNode is ClassDeclarationSyntax classDeclaration))
            return;

         if (IsSetupClass(classDeclaration))
            SetupCandidates.Add(classDeclaration);
      }

      private static bool IsSetupClass(ClassDeclarationSyntax classDeclaration)
      {
         if(HasAttributes(classDeclaration) && IsPartial(classDeclaration))
            return true;
         return false;
      }

      private static bool HasAttributes(ClassDeclarationSyntax classDeclaration)
      {
         return classDeclaration.AttributeLists.Count > 0;
      }

      private static bool IsPartial(ClassDeclarationSyntax classDeclarationSyntax)
      {
         return true;
         foreach (var modifier in classDeclarationSyntax.DescendantNodes())
         {
            
         }

         var parentTrivia = classDeclarationSyntax.ParentTrivia;

         foreach (var token in classDeclarationSyntax.ChildTokens())
         {
            var isKind = token.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PartialKeyword);

            if (isKind)
               return true;
         }

         return false;
      }
   }
}