// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupModel.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System.Collections;
   using System.Collections.Generic;
   using System.Linq;

   internal class SetupModel
   {
      public IList<SetupClassModel> SetupClasses { get; private set; }

      public IList<SetupEntryClassModel> EntryClasses { get; private set; }

      public static SetupModel Create(FluentGeneratorContext context, SetupClassInfo[] fluentSetupClasses)
      {
         var classModels = new List<SetupClassModel>(fluentSetupClasses.Length);
         foreach (var setupClassInfo in fluentSetupClasses)
         {
            var classModel = SetupClassModel.Create(context, setupClassInfo);
            classModels.Add(classModel);
         }
         
         return new SetupModel
         {
            DefaultEntryNamespace = context.Compilation.Assembly.MetadataName,
            SetupClasses = classModels,
            EntryClasses = CreateEntryClasses(classModels).ToArray()
         };
      }

      public string DefaultEntryNamespace { get; set; }

      private static IEnumerable<SetupEntryClassModel> CreateEntryClasses(IEnumerable<SetupClassModel> setupClasses)
      {
         foreach (var namespaceGroup in setupClasses.GroupBy(x => x.EntryClassNamespace))
         {
            var includingNamespace = namespaceGroup.Key;

            foreach (var setupClassModel in namespaceGroup.GroupBy(x => x.EntryClassName))
            {
               yield return new SetupEntryClassModel
               {
                  ContainingNamespace = includingNamespace,
                  ClassName = setupClassModel.Key,
                  SetupClasses = setupClassModel.ToArray() 
               };
            }
         }
      }
   }
}