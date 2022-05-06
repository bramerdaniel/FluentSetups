// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupClassInfoSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.Setups;

using FluentSetups.SourceGenerator;

internal class SetupClassInfoSetup : SetupBase
{
   #region Public Methods and Operators

   public SetupClassInfoSetup WithRootNamespace(string value)
   {
      RootNamespace = value;
      return this;
   }

   public SetupClassInfoSetup WithSource(string code)
   {
      AddSource(code);
      return this;
   }
   
   public SetupClassInfo Done()
   {
      var compilation = CreateCompilation();
      
      var syntaxWalker = new SyntaxHelper();
      syntaxWalker.Visit(SyntaxTrees[0].GetRoot());
      
      var context = FluentGeneratorContext.FromCompilation(compilation);
      return context.CreateFluentSetupInfo(FirstClassDeclarationSyntax());
   }

   #endregion
   
}