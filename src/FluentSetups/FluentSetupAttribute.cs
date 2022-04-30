namespace FluentSetups
{
   using System;

   [AttributeUsage(AttributeTargets.Class)]
   public class FluentSetupAttribute : Attribute
   {
      /// <summary>Gets the name of the setup class this class is generated inside.</summary>
      public string SetupClassName { get; }

      public FluentSetupAttribute(string setupClassName)
      {
         SetupClassName = setupClassName ?? throw new ArgumentNullException(nameof(setupClassName));
      }

      public FluentSetupAttribute()
      : this("Setup")
      {

      }
   }
}