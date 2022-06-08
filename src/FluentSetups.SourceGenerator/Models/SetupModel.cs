// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupModel.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System.Collections.Generic;
   using System.Linq;

   using Microsoft.CodeAnalysis;

   internal class SetupModel
   {
      #region Public Properties

      public string DefaultEntryNamespace { get; set; }

      public IList<EEntryClass> EntryClasses { get; private set; }

      public IList<FClass> SetupClasses { get; private set; }

      #endregion

      #region Public Methods and Operators

      public static SetupModel Create(FluentGeneratorContext context, SetupClassInfo[] fluentSetupClasses)
      {
         var classModels = new List<FClass>(fluentSetupClasses.Length);
         foreach (var setupClassInfo in fluentSetupClasses)
         {
            var classModel = new FClass(context, setupClassInfo.ClassSymbol, setupClassInfo.FluentSetupAttribute);
            classModels.Add(classModel);
         }

         return new SetupModel
         {
            DefaultEntryNamespace = context.Compilation.Assembly.MetadataName,
            SetupClasses = classModels,
            EntryClasses = CreateEntryClasses(classModels, context).ToArray()
         };
      }

      #endregion

      #region Methods

      private static IEnumerable<EEntryClass> CreateEntryClasses(IEnumerable<FClass> setupClasses, FluentGeneratorContext context)
      {
         foreach (var namespaceGroup in setupClasses.GroupBy(x => x.EntryClassNamespace))
         {
            var includingNamespace = namespaceGroup.Key;

            foreach (var setupClassModels in namespaceGroup.GroupBy(x => x.EntryClassName))
            {
               var existingClass = FindExistingClass(context, includingNamespace, setupClassModels.Key);
               if (existingClass != null)
               {
                  yield return new EEntryClass(existingClass, setupClassModels.ToArray());
               }
               else
               {
                  yield return new EEntryClass(includingNamespace, setupClassModels.Key, setupClassModels.ToArray());
               }
            }
         }
      }

      private static INamedTypeSymbol FindExistingClass(FluentGeneratorContext context, string containingNamespace, string className)
      {
         var fullyQualifiedMetadataName = string.IsNullOrWhiteSpace(containingNamespace)
            ? className 
            : $"{containingNamespace}.{className}";

         return context.Compilation.GetTypeByMetadataName(fullyQualifiedMetadataName);
      }

      #endregion
   }
}