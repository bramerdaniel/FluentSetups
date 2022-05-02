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
   using Microsoft.CodeAnalysis.Text;

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

         // GenerateSetupEntryClasses(fluentSetupClasses);
      }

      private void GenerateSetupEntryClasses(SetupClassInfo[] setupClassInfos)
      {
         if (setupClassInfos.Length == 0)
            return;

         foreach (var entryNameSpace in setupClassInfos.GroupBy(x => x.GetSetupEntryNameSpace()))
         {
            var entryBuilder = new StringBuilder();
            entryBuilder.AppendLine($"namespace {entryNameSpace.Key}");
            entryBuilder.AppendLine("{");

            foreach (var classInfos in setupClassInfos.GroupBy(x => x.GetSetupEntryClassName()))
            {
               var existing = Context.Compilation.GetTypeByMetadataName(classInfos.Key);
               if (existing == null)
               {
                  entryBuilder.AppendLine($"   internal partial class {classInfos.Key}");
                  entryBuilder.AppendLine("   {");
                  foreach (var classInfo in classInfos)
                  {
                     entryBuilder.AppendLine($"      internal static {classInfo.ClassName} {classInfo.ClassName}() => new {classInfo.ClassName}();");
                     entryBuilder.AppendLine();
                  }

                  entryBuilder.AppendLine("   }");
                  entryBuilder.AppendLine();
               }
            }

            entryBuilder.AppendLine("}");

            Context.AddSource($"{entryNameSpace.Key}.Setups", SourceText.From(entryBuilder.ToString(), Encoding.UTF8));
         }
      }
   }
}