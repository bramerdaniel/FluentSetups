// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenerationRun.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using System.Linq;
   using System.Text;

   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp.Syntax;

   internal struct GenerationRun
   {
      public GeneratorExecutionContext Context { get; }

      public GenerationRun(GeneratorExecutionContext context)
      {
         Context = context;
      }

      public void Execute(SetupClassInfo[] fluentSetupClasses)
      {
         foreach (var fluentSetupClass in fluentSetupClasses)
         {
            var generator = new FluentSetupClassGenerator(Context, fluentSetupClass.ClassSyntax);
            generator.Execute();
         }

         GenerateSetupEntryClasses(fluentSetupClasses);
      }

      private void GenerateSetupEntryClasses(SetupClassInfo[] setupClassInfos)
      {
         var entryBuilder = new StringBuilder();

         foreach (var classInfos in setupClassInfos.GroupBy(GetSetupEntryClassName))
         {
            var existing = Context.Compilation.GetTypeByMetadataName(classInfos.Key);
            entryBuilder.AppendLine($"internal partial class {classInfos.Key}");
            entryBuilder.AppendLine("{");
            foreach (var classInfo in classInfos)
            {
               entryBuilder.AppendLine($"   internal static {classInfo.ClassName} {classInfo.ClassName}() => new {classInfo.ClassName}();");
               entryBuilder.AppendLine();

            }
            entryBuilder.AppendLine("}");

         }

      }

      private static string GetSetupEntryClassName(SetupClassInfo setupClassInfo)
      {
         var firstArgument = setupClassInfo.FluentSetupAttribute.ConstructorArguments.FirstOrDefault();
         if (firstArgument.IsNull)
            return "Setup";

         return firstArgument.Value?.ToString() ?? "Setup";

      }
   }
}