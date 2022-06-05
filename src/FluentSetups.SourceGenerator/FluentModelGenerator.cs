// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentModelGenerator.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Linq;
   using System.Text;

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

      private GeneratedSource GenerateEntryClass(SetupEntryClassModel classModel)
      {
         var source = new GeneratedSource { Name = $"{classModel.ClassName}.generated.cs" };
         try
         {
            var sourceBuilder = new StringBuilder();
            sourceBuilder.AppendLine($"namespace {classModel.ContainingNamespace}");
            sourceBuilder.AppendLine("{");
            GenerateRequiredNamespaces(classModel, sourceBuilder);

            sourceBuilder.AppendLine("/// <summary>Automatic generated class part by fluent setups</summary>");
            sourceBuilder.AppendLine("[CompilerGenerated]");
            sourceBuilder.AppendLine($"{classModel.Modifier} partial class {classModel.ClassName}");
            sourceBuilder.AppendLine("{");
            GenerateEntryPoints(classModel, sourceBuilder);
            sourceBuilder.AppendLine("}");

            sourceBuilder.AppendLine("}");
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceBuilder.ToString()).GetRoot().NormalizeWhitespace();
            source.Code = syntaxTree.ToString();
         }
         catch (Exception e)
         {
            ReportUnknownError(source, e);
         }

         return source;
      }

      private void GenerateEntryPoints(SetupEntryClassModel classModel, StringBuilder sourceBuilder)
      {
         var addedMethods = new HashSet<string>();
         foreach (var setupClass in classModel.SetupClasses)
         {
            if (setupClass.GenerationMode.HasFlag(GeneratorMode.EntryMethod))
            {
               var requestedName = setupClass.SetupMethod;
               var generatedName = GetName(addedMethods, requestedName);

               sourceBuilder.AppendLine($"/// <summary>Creates a new setup for the {setupClass.ClassName} class</summary>");
               sourceBuilder.Append($"{classModel.Modifier} static {setupClass.ClassName} {generatedName}()");
               sourceBuilder.AppendLine($" => new {setupClass.ClassName}();");
               sourceBuilder.AppendLine();
            }
         }
      }

      private string GetName(HashSet<string> existingMethods, string requestedName)
      {
         var name = requestedName;
         var counter = 1;
         while (existingMethods.Contains(name))
            name = $"{requestedName}{counter++}";

         existingMethods.Add(name);
         return name;
      }

      private void GenerateRequiredNamespaces(SetupEntryClassModel classModel, StringBuilder sourceBuilder)
      {
         sourceBuilder.AppendLine("using System.Runtime.CompilerServices;");

         var namespaces = classModel.SetupClasses.Where(x => !string.IsNullOrWhiteSpace(x.ContainingNamespace))
            .Select(x => x.ContainingNamespace).Distinct();

         foreach (var requiredNamespace in namespaces)
            sourceBuilder.AppendLine($"using {requiredNamespace};");
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