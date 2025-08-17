namespace McWuT.Common.Converters;

public sealed class JsonToCSharpOptions
{
    public string RootClassName { get; set; } = "Root";
    public bool IncludeJsonPropertyNameAttributes { get; set; } = true;

    // New options
    public string? NamespaceName { get; set; }
    public bool UseFileScopedNamespace { get; set; } = false;
    public bool UseNullableReferenceTypes { get; set; } = true;
    public bool UseIsoDateTimeConverterAttribute { get; set; } = false;
}
