namespace FluentSetups.IntegrationTests.Targets;
using System.Collections.Generic;

public record Room(IEnumerable<Person> People);

public class Person
{
   #region Public Properties

   public int Age { get; set; }

   public string FirstName { get; set; }

   public string LastName { get; set; }

   #endregion
}