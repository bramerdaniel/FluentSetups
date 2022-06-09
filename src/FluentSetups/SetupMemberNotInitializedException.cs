// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupMemberNotInitializedException.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups
{
   using System;
   using System.Diagnostics.CodeAnalysis;

   using JetBrains.Annotations;

   [SuppressMessage("ReSharper", "UnusedMember.Global")]
   [UsedImplicitly]
   public class SetupMemberNotInitializedException : Exception
   {
      /// <summary>Gets the name of the member.</summary>
      [UsedImplicitly]
      public string MemberName { get; }

      #region Constructors and Destructors

      public SetupMemberNotInitializedException(string memberName)
         : base(CreateMessage(memberName))
      {
         MemberName = memberName;
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