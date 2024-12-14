
<p align="center">
  <a href="https://github.com/oknozor/CartographeAutomatique/actions"
    ><img
      src="https://github.com/oknozor/CartographeAutomatique/actions/workflows/ci.yml/badge.svg"
      alt="GitHub Actions workflow status"
  /></a>
  <br />
  <a href="https://conventionalcommits.org"
    ><img
      src="https://img.shields.io/badge/Conventional%20Commits-1.0.0-yellow.svg"
      alt="Conventional commits"
  /></a>
  <a href="https://github.com/oknozor/CartographeAutomatique/blob/main/LICENSE"
    ><img
      src="https://img.shields.io/github/license/oknozor/CartographeAutomatique"
      alt="Repository license"
  /></a>
</p>
<p align="center">
  <a href="https://oknozor.github.io/CartographeAutomatique/">Documentation</a>
</p>

# Cartographe Automatique
Cartographe Automatique is a Roselyn SourceGenerator designed to streamline the process of mapping between C# types. By annotating your source and target classes with specific attributes, it generates type-safe, performant, and dependency-free mapping methods. This eliminates boilerplate code and ensures maintainable, consistent mapping logic throughout your application.
___

## What is Cartographe Automatique ? 

CartographeAutomatique is a Roselyn source generator designed to generate type-safe
and high performance mapper for C# classes.
By automating the creation of mappings, CartographeAutomatique eliminates the need for tedious and error-prone manual coding.
The generator provides sensible defaults and built-in type conversions, allowing it to handle standard mappings effortlessly, while also offering flexibility for custom configurations or specialized mapping behaviors.
CartographeAutomatique can map between conventional classes, records, and even complex hierarchies, making it an adaptable tool for diverse C# applications.


To create a mapping between two types, simply add the following attribute to your class:

```csharp
[MapTo(typeof(Point3))]
public class Vector3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}
```

Read the [documentation](https://oknozor.github.io/CartographeAutomatique/) for more.

## Licence
All the code in this repository is released under the MIT License, for more information take a look at the [LICENSE](LICENCE) file.