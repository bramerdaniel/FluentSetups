// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentModelGenerator.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
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

         return source;
      }

      private void GenerateRequiredNamespaces(SetupEntryClassModel classModel, StringBuilder sourceBuilder)
      {
         sourceBuilder.AppendLine("using System.Runtime.CompilerServices;");

         var enumerable = classModel.SetupClasses.Select(x => x.ContainingNamespace);
         foreach (var requiredNamespace in enumerable)
            sourceBuilder.AppendLine($"using {requiredNamespace};");
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
         var sourceBuilder = new StringBuilder();
         sourceBuilder.AppendLine($"namespace {classModel.ContainingNamespace}");
         sourceBuilder.AppendLine("{");
         sourceBuilder.AppendLine("using System;");
         sourceBuilder.AppendLine("using System.Runtime.CompilerServices;");

         sourceBuilder.AppendLine("/// <summary>Automatic generated class part by fluent setups</summary>");
         sourceBuilder.AppendLine("[CompilerGenerated]");
         sourceBuilder.AppendLine($"{classModel.Modifier} partial class {classModel.ClassName}");
         sourceBuilder.AppendLine("{");
         GenerateSetupMembers(classModel, sourceBuilder);
         sourceBuilder.AppendLine("}");

         sourceBuilder.AppendLine("}");
         var syntaxTree = CSharpSyntaxTree.ParseText(sourceBuilder.ToString()).GetRoot().NormalizeWhitespace();
         source.Code = syntaxTree.ToString();
         return source;
      }

      private void GenerateSetupMembers(SetupClassModel classModel, StringBuilder sourceBuilder)
      {
         foreach (var member in classModel.Fields)
            GenerateMemberSetup(classModel, sourceBuilder, member);

         foreach (var member in classModel.Properties)
            GenerateMemberSetup(classModel, sourceBuilder, member);

      }

      private static void GenerateMemberSetup(SetupClassModel classModel, StringBuilder sourceBuilder, SetupMemberModel memberModel)
      {

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
         sourceBuilder.AppendLine($"{classModel.Modifier} {memberModel.TypeName} Get{memberModel.UpperMemberName}OrThrow()");
         sourceBuilder.AppendLine($"=> {memberModel.MemberSetFieldName} ? {memberModel.MemberName} : throw new InvalidOperationException(\"The member {memberModel.MemberName} was not set.\");");

         sourceBuilder.AppendLine();
         sourceBuilder.AppendLine($"{classModel.Modifier} {memberModel.TypeName} Get{memberModel.UpperMemberName}OrDefault(Func<{memberModel.TypeName}> defaultValue)");
         sourceBuilder.AppendLine($" => {memberModel.MemberSetFieldName} ? {memberModel.MemberName} : defaultValue();");
      }
   }
}