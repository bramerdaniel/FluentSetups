﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentModelGenerator.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;

   using FluentSetups.SourceGenerator.Models;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp;

   internal class FluentModelGenerator
   {
      public IEnumerable<GeneratedSource> Execute(SetupModel setupModel)
      {
         foreach (var setupClass in setupModel.SetupClasses)
            yield return GenerateSetupClass(setupClass);

         foreach (var entryClass in setupModel.EntryClasses)
            yield return GenerateEntryClass(entryClass);
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
            ReportError(source, e);
         }

         return source;
      }

      private void GenerateRequiredNamespaces(SetupEntryClassModel classModel, StringBuilder sourceBuilder)
      {
         sourceBuilder.AppendLine("using System.Runtime.CompilerServices;");

         var enumerable = classModel.SetupClasses.Where(x => !string.IsNullOrWhiteSpace(x.ContainingNamespace)).Select(x => x.ContainingNamespace);
         foreach (var requiredNamespace in enumerable)
            sourceBuilder.AppendLine($"using {requiredNamespace};");
      }
      private void GenerateRequiredNamespaces(SetupClassModel classModel, StringBuilder sourceBuilder)
      {
         sourceBuilder.AppendLine("using System;");
         sourceBuilder.AppendLine("using System.Runtime.CompilerServices;");
         if (TargetCreationPossible(classModel) && !string.IsNullOrWhiteSpace(classModel.TargetTypeNamespace))
         {
            sourceBuilder.AppendLine($"using {classModel.TargetTypeNamespace};");
         }
      }

      private void GenerateEntryPoints(SetupEntryClassModel classModel, StringBuilder sourceBuilder)
      {
         foreach (var setupClass in classModel.SetupClasses)
         {
            sourceBuilder.AppendLine($"/// <summary>Creates a new setup for the {setupClass.ClassName} class</summary>");
            sourceBuilder.Append($"{classModel.Modifier} static {setupClass.ClassName} {ComputeEntryMethodName(setupClass.ClassName)}()");
            sourceBuilder.AppendLine($" => new {setupClass.ClassName}();");
            sourceBuilder.AppendLine();
         }
      }

      private static string ComputeEntryMethodName(string className)
      {
         if (className.EndsWith("Setup"))
            return className.Substring(0, className.Length - 5);

         return className;
      }

      private GeneratedSource GenerateSetupClass(SetupClassModel classModel)
      {
         var source = new GeneratedSource { Name = $"{classModel.ClassName}.generated.cs" };
         try
         {
            var sourceBuilder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(classModel.ContainingNamespace))
            {
               sourceBuilder.AppendLine($"namespace {classModel.ContainingNamespace}");
               sourceBuilder.AppendLine("{");
            }

            GenerateRequiredNamespaces(classModel, sourceBuilder);

            sourceBuilder.AppendLine("/// <summary>Automatic generated class part by fluent setups</summary>");
            sourceBuilder.AppendLine("[CompilerGenerated]");
            sourceBuilder.AppendLine($"{classModel.Modifier} partial class {classModel.ClassName}");
            sourceBuilder.AppendLine("{");
            GenerateSetupMembers(classModel, sourceBuilder);
            sourceBuilder.AppendLine("}");

            if (!string.IsNullOrWhiteSpace(classModel.ContainingNamespace))
               sourceBuilder.AppendLine("}");
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceBuilder.ToString()).GetRoot().NormalizeWhitespace();
            source.Code = syntaxTree.ToString();
         }
         catch (Exception e)
         {
            ReportError(source, e);
         }

         return source;
      }

      private static void ReportError(GeneratedSource source, Exception e)
      {
         var missingReference = new DiagnosticDescriptor(id: "FS0002", title: "fluent source generator failed",
            messageFormat: "Error while generating source '{0}'. Message: {1}",
            category: nameof(FluentSetupSourceGenerator),
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

         source.Error = Diagnostic.Create(missingReference, Location.None, source.Name, e.Message);
      }

      private void GenerateSetupMembers(SetupClassModel classModel, StringBuilder sourceBuilder)
      {
         foreach (var member in classModel.Fields)
            GenerateMemberSetup(classModel, sourceBuilder, member, false);

         foreach (var member in classModel.Properties)
            GenerateMemberSetup(classModel, sourceBuilder, member, false);

         if (classModel.Target != null)
            foreach (var member in classModel.Target.Members)
               GenerateMemberSetup(classModel, sourceBuilder, member, true);

         GenerateTargetCreation(classModel, sourceBuilder);
      }

      private void GenerateTargetCreation(SetupClassModel classModel, StringBuilder sourceBuilder)
      {
         if (!TargetCreationPossible(classModel))
            return;

         if (ContainsUserDefinedTargetBuilder(classModel))
            return;

         sourceBuilder.AppendLine($"public {classModel.TargetTypeName} Done()");
         sourceBuilder.AppendLine("{");
         sourceBuilder.AppendLine($"   var target = new {classModel.TargetTypeName}();");
         sourceBuilder.AppendLine($"   return target;");
         sourceBuilder.AppendLine("}");
      }

      private bool ContainsUserDefinedTargetBuilder(SetupClassModel classModel)
      {
         return classModel.Methods.Any(m => m.MemberName == "Done" && m.TypeName == null);
      }

      private static bool TargetCreationPossible(SetupClassModel classModel)
      {
         if (classModel.TargetMode == TargetGenerationMode.Disabled)
            return false;

         return !string.IsNullOrWhiteSpace(classModel.TargetTypeName);
      }

      private static void GenerateMemberSetup(SetupClassModel classModel, StringBuilder sourceBuilder, FMember memberModel, bool createMember)
      {
         if (createMember)
         {
            if (classModel.Properties.Any(f => f.MemberName == memberModel.MemberName))
               return;

            if (classModel.Fields.Any(f => f.MemberName.Equals(memberModel.MemberName, StringComparison.InvariantCultureIgnoreCase)))
               return;

            if (ContainsUserdefinedMethod(classModel, memberModel))
               return;

            sourceBuilder.AppendLine($"private {memberModel.TypeName} {memberModel.MemberName};");
         }

         sourceBuilder.AppendLine($"{classModel.Modifier} {classModel.ClassName} {memberModel.SetupMethodName}({memberModel.TypeName} value)");
         sourceBuilder.AppendLine("{");

         ////var validation = CreateParameterValidation(propertySymbol, "value");
         ////if (!string.IsNullOrWhiteSpace(validation))
         ////   sourceBuilder.AppendLine(validation);

         sourceBuilder.AppendLine($"{memberModel.MemberName} = value;");
         sourceBuilder.AppendLine($"{memberModel.MemberSetFieldName} = true;");
         sourceBuilder.AppendLine("return this;");
         sourceBuilder.AppendLine("}");
         sourceBuilder.AppendLine();

         sourceBuilder.AppendLine($"private bool {memberModel.MemberSetFieldName};");
         sourceBuilder.AppendLine($"protected {memberModel.TypeName} Get{memberModel.UpperMemberName}OrThrow()");
         sourceBuilder.AppendLine("{");
         sourceBuilder.AppendLine($"return {memberModel.MemberSetFieldName} ? {memberModel.MemberName} : throw new InvalidOperationException(\"The member {memberModel.MemberName} was not set.\");");
         sourceBuilder.AppendLine("}");

         sourceBuilder.AppendLine();
         sourceBuilder.AppendLine($"protected {memberModel.TypeName} Get{memberModel.UpperMemberName}OrDefault(Func<{memberModel.TypeName}> defaultValue)");
         sourceBuilder.AppendLine("{");
         sourceBuilder.AppendLine($"return {memberModel.MemberSetFieldName} ? {memberModel.MemberName} : defaultValue();");
         sourceBuilder.AppendLine("}");
      }

      private static bool ContainsUserdefinedMethod(SetupClassModel classModel, FMember memberModel)
      {
         foreach (var method in classModel.Methods)
         {
            if (method.SetupMethodName == memberModel.SetupMethodName && method.TypeName == memberModel.TypeName)
               return true;
         }

         return false;
      }
   }
}