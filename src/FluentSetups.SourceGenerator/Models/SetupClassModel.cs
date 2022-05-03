// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupClassModel.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System.Collections.Generic;
   using System.Linq;

   using Microsoft.CodeAnalysis;

   /// <summary>Model describing a setup class</summary>
   internal class SetupClassModel
   {
      public static SetupClassModel Create(FluentGeneratorContext context, SetupClassInfo classInfo)
      {
         var classModel = new SetupClassModel
         {
            Context = context,
            ClassName = classInfo.ClassName,
            ContainingNamespace = ComputeNamespace(classInfo),
            EntryClassNamespace = classInfo.GetSetupEntryNameSpace(),
            EntryClassName = classInfo.GetSetupEntryClassName()
         };

         classModel.FillProperties(classInfo);

         return classModel;
      }

      private void FillProperties(SetupClassInfo classInfo)
      {
         Properties = ComputePropertySetups(classInfo).ToArray();
      }

      private IEnumerable<SetupPropertyModel> ComputePropertySetups(SetupClassInfo classInfo)
      {
         foreach (var propertySymbol in classInfo.ClassSymbol.GetMembers().OfType<IPropertySymbol>())
         {
            if (TryCreateProperty(propertySymbol, out var propertyModel))
               yield return propertyModel;
         }
      }

      private bool TryCreateProperty(IPropertySymbol propertySymbol, out SetupPropertyModel propertyModel)
      {
         foreach (var attribute in propertySymbol.GetAttributes().Where(x => x.AttributeClass != null))
         {
            if (Context.FluentPropertyAttribute.Equals(attribute.AttributeClass, SymbolEqualityComparer.Default))
            {
               propertyModel = SetupPropertyModel.Create(this, propertySymbol, attribute);
               return true;
            }
         }

         propertyModel = null;
         return false;
      }

      public IReadOnlyList<SetupPropertyModel> Properties { get; set; }

      private static string ComputeNamespace(SetupClassInfo classInfo)
      {
         var namespaceSymbol = classInfo.ClassSymbol.ContainingNamespace;
         if (namespaceSymbol.IsGlobalNamespace)
            return null;

         return namespaceSymbol.ToString();
      }

      public FluentGeneratorContext Context { get; private set; }

      public string ClassName { get; private set; }

      public string ContainingNamespace { get; private set; }

      public string Modifier { get; set; } = "public";

      public string EntryClassName { get; set; }

      public string EntryClassNamespace { get; set; }
   }
}