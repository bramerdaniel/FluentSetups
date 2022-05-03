// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupClassInfoSetup.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.UnitTests.Setups;

using FluentSetups.SourceGenerator;

internal class SetupClassInfoSetup : SetupBase
{


   #region Public Properties

   public string ClassName { get; private set; }

   #endregion

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
      
      var api = FluentGeneratorContext.FromCompilation(compilation);
      var semanticModel = compilation.GetSemanticModel(SyntaxTrees[0]);
      return new SetupClassInfo(api, FirstClassDeclarationSyntax(), semanticModel);
   }

   public SetupClassInfoSetup WithName(string className)
   {
      ClassName = className;
      return this;
   }

   #endregion
   
}