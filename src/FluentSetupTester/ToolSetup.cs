namespace FluentSetupTester
{
   using FluentSetups;

   [FluentSetup(EntryNamespace = "MySetups")]
   // [FluentSetup("Hubert", EntryNamespace = "MySetups")]
   public partial class ToolSetup
   {
      #region Public Properties

      [FluentMember] public string Name { get; set; }

      [FluentMember] public int Number { get; set; }

      #endregion

      #region Methods

      internal string CreateInstance()
      {
         return $"{Name} => {Number}";
      }

      #endregion
   }
}