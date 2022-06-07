// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EEntryMethod.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using System.Diagnostics;
   using System.Text;

   [DebuggerDisplay("{Signature}")]
   internal class EEntryMethod : MethodBase
   {
      #region Constants and Fields

      private readonly IFluentSetupClass setupClass;

      #endregion

      #region Constructors and Destructors

      /// <summary>Initializes a new instance of the <see cref="EEntryMethod"/> class.</summary>
      /// <param name="entryClass">The entry class that owns the method.</param>
      /// <param name="setupClass">The setup class that will be created.</param>
      public EEntryMethod(IFluentEntryClass entryClass, IFluentSetupClass setupClass)
         : base(entryClass, setupClass.EntryMethod)
      {
         this.setupClass = setupClass;
      }

      #endregion

      #region Public Properties

      public override bool IsUserDefined => false;

      public override int ParameterCount => 0;

      #endregion

      #region Public Methods and Operators

      public override string ToCode()
      {
         var sourceBuilder = new StringBuilder();
         AppendDocumentation(sourceBuilder);

         sourceBuilder.Append($"{setupClass.Modifier} static {setupClass.ClassName} {Name}()");
         sourceBuilder.AppendLine($" => new {setupClass.ClassName}();");
         sourceBuilder.AppendLine();
         return sourceBuilder.ToString();
      }

      #endregion
   }
}