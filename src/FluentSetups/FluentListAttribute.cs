// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentListAttribute.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups
{
   using System;

   [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
   public class FluentListAttribute : FluentMemberAttribute
   {
   }
}