using System.Text.Json;

namespace McWuT.Common.Converters;

public interface IJsonCSharpConversionService
{
    /// <summary>
    /// Generates C# class code from a JSON sample.
    /// </summary>
    /// <param name="json">JSON text</param>
    /// <param name="rootClassName">Optional root class name. Defaults to "Root"</param>
    /// <returns>Full C# code containing the root class and any nested classes.</returns>
    string GenerateCSharpFromJson(string json, string rootClassName = "Root");

    /// <summary>
    /// Generates a sample JSON object based on a C# class declaration.
    /// </summary>
    /// <param name="csharp">C# class code</param>
    /// <returns>Sample JSON string</returns>
    string GenerateSampleJsonFromCSharp(string csharp);

    /// <summary>
    /// Generates C# class code from a JSON sample with options.
    /// </summary>
    /// <param name="json">JSON text</param>
    /// <param name="options">Options for JSON to C# generation</param>
    /// <returns>Full C# code containing the root class and any nested classes.</returns>
    string GenerateCSharpFromJson(string json, JsonToCSharpOptions options);

    /// <summary>
    /// Generates a sample JSON object based on a C# class declaration with a specific target type.
    /// </summary>
    /// <param name="csharp">C# class code</param>
    /// <param name="targetTypeName">Optional target type name</param>
    /// <returns>Sample JSON string</returns>
    string GenerateSampleJsonFromCSharp(string csharp, string? targetTypeName);

    /// <summary>
    /// Discovers the type names defined in the given C# class code.
    /// </summary>
    /// <param name="csharp">C# class code</param>
    /// <returns>A read-only list of discovered type names</returns>
    IReadOnlyList<string> DiscoverTypeNames(string csharp);
}
