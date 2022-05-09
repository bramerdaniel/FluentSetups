// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FMethod.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator.Models
{
   using Microsoft.CodeAnalysis;

   internal class FMethod : FMember
   {
      #region Public Properties
      
      #endregion

      #region Public Methods and Operators

      public static FMethod Create(IMethodSymbol methodSymbol)
      {
         return new FMethod
         {
            MemberName = methodSymbol.Name, 
            SetupMethodName = methodSymbol.Name,
            TypeName = methodSymbol.Parameters[0].Type.ToString()
         };
      }



      #endregion

      #region Methods

      #endregion
   }
}