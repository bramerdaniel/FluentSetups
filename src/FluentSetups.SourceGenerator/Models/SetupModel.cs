// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupModel.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
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

      public IList<SetupEntryClassModel> EntryClasses { get; private set; }

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

      internal static string ComputeModifier(INamedTypeSymbol typeSymbol)
      {
         if (typeSymbol != null)
         {
            switch (typeSymbol.DeclaredAccessibility)
            {
               case Accessibility.NotApplicable:
               case Accessibility.Private:
                  return "private";
               case Accessibility.ProtectedAndInternal:
                  return "protected internal";
               case Accessibility.Protected:
                  return "protected";
               case Accessibility.Internal:
                  return "internal";
               case Accessibility.ProtectedOrInternal:
                  return "internal";
               case Accessibility.Public:
                  return "public";
            }
         }

         return "internal";
      }

      internal static string ComputeModifier(string targetNamespace, string targetName, FluentGeneratorContext context)
      {
         var existingType = context.Compilation.GetTypeByMetadataName($"{targetNamespace}.{targetName}");
         return ComputeModifier(existingType);
      }

      private static IEnumerable<SetupEntryClassModel> CreateEntryClasses(IEnumerable<FClass> setupClasses, FluentGeneratorContext context)
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
                  SetupClasses = setupClassModel.ToArray(),
                  Modifier = ComputeModifier(includingNamespace, setupClassModel.Key, context)
               };
            }
         }
      }

      #endregion
   }
}