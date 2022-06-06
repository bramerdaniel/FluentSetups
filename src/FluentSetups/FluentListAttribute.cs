// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentListAttribute.cs" company="KUKA Deutschland GmbH">
//   Copyright (c) KUKA Deutschland GmbH 2006 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups
{
   using System;

   [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
   public class FluentListAttribute: FluentMemberAttribute
   {
   }
}