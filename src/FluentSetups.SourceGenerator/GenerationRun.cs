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

      public FluentApi FluentApi { get; }

      public GenerationRun(GeneratorExecutionContext context, FluentApi fluentApi)
      {
         Context = context;
         FluentApi = fluentApi;
      }

      public void Execute(SetupClassInfo[] fluentSetupClasses)
      {
         foreach (var fluentSetupClass in fluentSetupClasses)
         {
            var generator = new FluentSetupClassGenerator(Context, fluentSetupClass.ClassSyntax, FluentApi);
            generator.Execute();
         }

         GenerateSetupEntryClasses(fluentSetupClasses);
      }

      private static string ComputeEntryMethodName(string className)
      {
         if (className.EndsWith("Setup"))
            return className.Substring(0, className.Length - 5);

         return className;
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
                     entryBuilder.AppendLine($"      internal static {classInfo.ClassName} {ComputeEntryMethodName(classInfo.ClassName)}() => new {classInfo.ClassName}();");
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