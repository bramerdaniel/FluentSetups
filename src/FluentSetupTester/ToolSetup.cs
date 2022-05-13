namespace FluentSetupTester
{
   using FluentSetups;

   [FluentSetup(typeof(Tool))]
   public partial class ToolSetup
   {
   }

   public class Tool
   {
      public string Name { get; set; }

      public int Number { get; set; }
   }
}