// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupClassModel.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System.Collections.Generic;
   using System.Linq;

   using Microsoft.CodeAnalysis;

   /// <summary>Model describing a setup class</summary>
   [System.Diagnostics.CodeAnalysis.SuppressMessage("sonar", "S3267:Loops should be simplified with \"LINQ\" expressions")]
   internal class SetupClassModel
   {
      #region Public Properties

      public string ClassName { get; private set; }

      public string ContainingNamespace { get; private set; }

      public FluentGeneratorContext Context { get; private set; }

      public string EntryClassName { get; set; }

      public string EntryClassNamespace { get; set; }

      public IReadOnlyList<SetupFieldModel> Fields { get; set; }

      public string Modifier { get; set; } = "public";

      public IReadOnlyList<SetupPropertyModel> Properties { get; set; }

      #endregion

      #region Public Methods and Operators

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

         classModel.FillMembers(classInfo);

         return classModel;
      }

      #endregion

      #region Methods

      private static string ComputeNamespace(SetupClassInfo classInfo)
      {
         var namespaceSymbol = classInfo.ClassSymbol.ContainingNamespace;
         if (namespaceSymbol.IsGlobalNamespace)
            return null;

         return namespaceSymbol.ToString();
      }

      private IEnumerable<SetupFieldModel> ComputeFieldSetups(SetupClassInfo classInfo)
      {
         foreach (var propertySymbol in classInfo.ClassSymbol.GetMembers().OfType<IFieldSymbol>())
         {
            if (TryCreateField(propertySymbol, out var propertyModel))
               yield return propertyModel;
         }
      }

      private IEnumerable<SetupPropertyModel> ComputePropertySetups(SetupClassInfo classInfo)
      {
         foreach (var propertySymbol in classInfo.ClassSymbol.GetMembers().OfType<IPropertySymbol>())
         {
            if (TryCreateProperty(propertySymbol, out var propertyModel))
               yield return propertyModel;
         }
      }

      private void FillMembers(SetupClassInfo classInfo)
      {
         Fields = ComputeFieldSetups(classInfo).ToArray();
         Properties = ComputePropertySetups(classInfo).ToArray();
      }

      private bool TryCreateField(IFieldSymbol fieldSymbol, out SetupFieldModel fieldModel)
      {
         foreach (var attribute in fieldSymbol.GetAttributes().Where(x => x.AttributeClass != null))
         {
            if (Context.FluentPropertyAttribute.Equals(attribute.AttributeClass, SymbolEqualityComparer.Default))
            {
               fieldModel = SetupFieldModel.Create(this, fieldSymbol, attribute);
               return true;
            }
         }

         fieldModel = null;
         return false;
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

      #endregion
   }
}