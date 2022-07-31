// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EEntryClass.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Linq;
   using System.Text;

   using Microsoft.CodeAnalysis;

   [DebuggerDisplay("{Modifier} class {ClassName} in {ContainingNamespace}")]
   internal class EEntryClass : IFluentEntryClass
   {
      #region Constructors and Destructors

      public EEntryClass(INamedTypeSymbol existingClass, IReadOnlyList<FClass> setupClasses)
      {
         ClassSymbol = existingClass ?? throw new ArgumentNullException(nameof(existingClass));
         ContainingNamespace = existingClass.ContainingNamespace?.ToString();
         ClassName = existingClass.Name;
         SetupClasses = setupClasses;

         Modifier = ComputeModifier(existingClass);
         Methods = ComputeMethods();
      }

      public EEntryClass(string includingNamespace, string className, IReadOnlyList<FClass> setupClasses)
      {
         ContainingNamespace = includingNamespace ?? throw new ArgumentNullException(nameof(includingNamespace));
         ClassName = className ?? throw new ArgumentNullException(nameof(className));
         SetupClasses = setupClasses ?? throw new ArgumentNullException(nameof(setupClasses));

         Modifier = "internal";
         Methods = ComputeMethods();
      }

      #endregion

      #region IFluentEntryClass Members

      /// <summary>Gets or sets the modifier.</summary>
      public string Modifier { get; }

      /// <summary>Gets the class symbol for the existing part of the setup class.</summary>
      public INamedTypeSymbol ClassSymbol { get; }

      /// <summary>Gets the name of the fluent entry class.</summary>
      public string ClassName { get; }

      #endregion

      #region Public Properties

      /// <summary>Gets the namespace the fluent entry class will be generated into.</summary>
      public string ContainingNamespace { get; }

      public IReadOnlyList<IFluentMethod> Methods { get; }

      /// <summary>Gets or sets all the <see cref="FClass"/>s that will be accessible from this entry class.</summary>
      public IReadOnlyList<FClass> SetupClasses { get; }

      #endregion

      #region Public Methods and Operators

      public string ToCode()
      {
         var sourceBuilder = new StringBuilder();
         sourceBuilder.AppendLine($"namespace {ContainingNamespace}");
         sourceBuilder.AppendLine("{");
         GenerateRequiredNamespaces(sourceBuilder);

         sourceBuilder.AppendLine("/// <summary>Automatic generated class part by fluent setups</summary>");
         sourceBuilder.AppendLine("[CompilerGenerated]");
         sourceBuilder.AppendLine($"{Modifier} partial class {ClassName}");
         sourceBuilder.AppendLine("{");
         GenerateEntryPoints(sourceBuilder);
         sourceBuilder.AppendLine("}");

         sourceBuilder.AppendLine("}");
         return sourceBuilder.ToString();
      }

      #endregion

      #region Methods

      private static string ComputeModifier(INamedTypeSymbol typeSymbol)
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

      private void AddExistingMethods(List<IFluentMethod> methods)
      {
         if (ClassSymbol == null)
            return;

         foreach (var methodSymbol in ClassSymbol.GetMembers().OfType<IMethodSymbol>())
         {
            var existingMethod = new FExistingMethod(this, methodSymbol);
            methods.Add(existingMethod);
         }
      }

      private void AddGeneratedMethods(List<IFluentMethod> methods)
      {
         foreach (var setupClass in SetupClasses)
         {
            var entryMethod = CreateEntryMethod(setupClass);
            if (entryMethod != null && !methods.Contains(entryMethod))
               methods.Add(entryMethod);
         }
      }

      private IReadOnlyList<IFluentMethod> ComputeMethods()
      {
         var methods = new List<IFluentMethod>(SetupClasses.Count);
         AddExistingMethods(methods);
         AddGeneratedMethods(methods);
         return methods;
      }

      private IFluentMethod CreateEntryMethod(FClass setupClass)
      {
         return setupClass.GenerationMode.HasFlag(GeneratorMode.EntryMethod)
            ? new EEntryMethod(this, setupClass)
            : null;
      }

      private void GenerateEntryPoints(StringBuilder sourceBuilder)
      {
         foreach (var method in Methods.Where(m => !m.IsUserDefined))
            sourceBuilder.AppendLine(method.ToCode());
      }

      private void GenerateRequiredNamespaces(StringBuilder sourceBuilder)
      {
         sourceBuilder.AppendLine("using System.Runtime.CompilerServices;");

         var namespaces = SetupClasses.Where(x => !string.IsNullOrWhiteSpace(x.ContainingNamespace))
            .Select(x => x.ContainingNamespace).Distinct();

         foreach (var requiredNamespace in namespaces)
            sourceBuilder.AppendLine($"using {requiredNamespace};");
      }

      private string GetName(HashSet<string> existingMethods, string requestedName)
      {
         var name = requestedName;
         var counter = 1;
         while (existingMethods.Contains(name))
            name = $"{requestedName}{counter++}";

         existingMethods.Add(name);
         return name;
      }

      #endregion
   }
}