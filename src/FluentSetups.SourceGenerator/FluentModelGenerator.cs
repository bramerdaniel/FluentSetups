// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentModelGenerator.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using System;
   using System.Collections.Generic;

   using FluentSetups.SourceGenerator.Models;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp;

   internal class FluentModelGenerator
   {
      #region Public Methods and Operators

      public IEnumerable<GeneratedSource> Execute(SetupModel setupModel)
      {
         foreach (var setupClass in setupModel.SetupClasses)
            yield return GenerateSetupClass(setupClass);

         foreach (var entryClass in setupModel.EntryClasses)
            yield return GenerateEntryClass(entryClass);
      }

      #endregion

      #region Methods

      private static void ReportUnknownError(GeneratedSource source, Exception e)
      {
         var missingReference = new DiagnosticDescriptor(id: "FSE0002", title: "FluentSetups source generator",
            messageFormat: "Error while generating source '{0}'. Message: {1}",
            category: nameof(FluentSetupSourceGenerator),
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

         source.AddDiagnostic(Diagnostic.Create(missingReference, Location.None, source.Name, e.Message));
      }

      private static void ReportIgnore(GeneratedSource source, FClass ignoredClass)
      {
         ////var location = CreateLocation(ignoredClass);
         ////source.AddDiagnostic(Diagnostic.Create(FluentSetupDiagnostics.MultiplePartialParts, location, ignoredClass.ClassName));
      }

      private static Location CreateLocation(FClass ignoredClass)
      {
         var reference = ignoredClass.FluentSetupAttribute.ApplicationSyntaxReference;
         if (reference == null)
            return Location.None;

         return Location.Create(reference.SyntaxTree, reference.Span);
      }

      private GeneratedSource GenerateEntryClass(EEntryClass classModel)
      {
         var source = new GeneratedSource { Name = $"{classModel.ClassName}.generated.cs" };
         try
         {
            var syntaxTree = CSharpSyntaxTree.ParseText(classModel.ToCode()).GetRoot().NormalizeWhitespace();
            source.Code = syntaxTree.ToString();
         }
         catch (Exception e)
         {
            ReportUnknownError(source, e);
         }

         return source;
      }

      private GeneratedSource GenerateSetupClass(FClass classModel)
      {
         var source = new GeneratedSource { Name = $"{classModel.ClassName}.generated.cs" };
         if (!classModel.GenerationMode.HasFlag(GeneratorMode.Setup))
         {
            source.Disable();
            ReportIgnore(source, classModel);
            return source;
         }

         try
         {
            var text = classModel.ToCode();
            var syntaxTree = CSharpSyntaxTree.ParseText(text).GetRoot().NormalizeWhitespace();
            source.Code = syntaxTree.ToString();
         }
         catch (Exception e)
         {
            ReportUnknownError(source, e);
         }

         return source;
      }

      #endregion
   }
}