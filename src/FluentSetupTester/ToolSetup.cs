namespace FluentSetupTester
{
   using FluentSetups;

   [FluentSetup("Hans")]
   public partial class ToolSetup : IFluentSetup<string>
   {
      #region Public Properties

      [FluentProperty] public string Name { get; set; }

      [FluentProperty] public int Number { get; set; }

      #endregion

      #region Methods

      internal string CreateInstance()
      {
         return $"{Name} => {Number}";
      }

      #endregion
   }
}