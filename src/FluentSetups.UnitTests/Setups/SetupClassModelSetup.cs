// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupClassModelSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.Setups;

using System;

using FluentSetups.SourceGenerator;
using FluentSetups.SourceGenerator.Models;

internal class SetupClassModelSetup : SetupBase
{
   #region Public Methods and Operators

   public FClass Done()
   {
      var compilation = CreateCompilation();
      var context = FluentGeneratorContext.FromCompilation(compilation);
      var setupClass = FirstClassDeclarationSyntax();
      if (setupClass == null)
         throw new InvalidOperationException("The parsed syntax was not a class");
      
      var classInfo = context.CreateFluentSetupInfo(setupClass);
      return new FClass(context, classInfo.ClassSymbol, classInfo.FluentSetupAttribute);
   }

   public SetupClassModelSetup FromSource(string code)
   {
      if (SyntaxTrees.Count > 0)
         throw new ArgumentException("Only one class is possible");

      AddSource(code);
      return this;
   }

   public SetupClassModelSetup WithRootNamespace(string rootNamespace)
   {
      RootNamespace = rootNamespace;
      return this;
   }

   #endregion
}