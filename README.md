# FluentSetups

## What is FluentSetups

FluentSetups is a library that that generates the boilerplate code for a builder pattern with a fluent api.
As I developed it especially for my unit tests...

## Usage 

Lets assume that you have an object you want to create a fluent setup api for


``` csharp

 // This is the object we want to create an fluent setup/builder for
 public class Person
 {
   public int Age { get; set; }

   public string FirstName { get; set; }

   public string LastName { get; set; }

   public string FullName => $"{FirstName} {LastName}";
 }

```

Normaly you would have to write all that boilerplate code, 
with FluentSetups all you have to write, is this.

``` csharp
 [FluentSetup(typeof(Person))]
 public partial class PersonSetup
 {
 }
```

