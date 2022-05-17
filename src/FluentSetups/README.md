# SourceGenerator for a fluent builder pattern

## Usage 

### 1. The target object we want to create an fluent builder for

``` csharp

 public class Person
 {
   public int Age { get; set; }

   public string FirstName { get; set; }

   public string LastName { get; set; }

   public string FullName => $"{FirstName} {LastName}";
 }

```
### 2. The code you have to write

``` csharp
 [FluentSetup(typeof(Person))]
 public partial class PersonSetup
 {
    // You can customize your fluent api here
    [FluentMember("HasAgeOf")]
    private int age;
 }
```

### 3. The generated api is ready to go

``` csharp
      var person = Setup.Person()
         .WithFirstName("Robert")
         .WithLastName("Ramirez")
         .HasAgeOf(34) // Customized fluent setup method
         .Done();
```