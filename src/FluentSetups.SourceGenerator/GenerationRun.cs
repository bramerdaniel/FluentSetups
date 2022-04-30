// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenerationRun.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp.Syntax;

   internal struct GenerationRun
   {
      public GeneratorExecutionContext Context { get; }

      public GenerationRun(GeneratorExecutionContext context)
      {
         Context = context;
      }

      public void Execute(ClassDeclarationSyntax[] fluentSetupClasses)
      {
         foreach (var fluentSetupClass in fluentSetupClasses)
         {
            var generator = new FluentSetupClassGenerator(Context, fluentSetupClass);
            generator.Execute();
         }
      }
   }
}