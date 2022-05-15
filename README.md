# FluentSetups

## What is FluentSetups

FluentSetups is a library that generates the boilerplate code for a fluent builder pattern ( I call it a fluent setup ).
It uses the roslyn source generators, to analyse your code and add the required setup classes, if you use the FluentSetups attributes in your code.

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

