// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupMemberNotInitializedException.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups
{
   using System;
   using System.Diagnostics.CodeAnalysis;

   [SuppressMessage("ReSharper", "UnusedMember.Global")]
   public class SetupMemberNotInitializedException : Exception
   {
      #region Constructors and Destructors

      public SetupMemberNotInitializedException(string memberName)
         : base(CreateMessage(memberName))
      {
      }

      #endregion

      #region Methods

      private static string CreateMessage(string memberName)
      {
         return $"The member '{memberName}' was not initialized from the fluent setup.";
      }

      #endregion
   }
}