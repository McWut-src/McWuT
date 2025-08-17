using McWuT.Common.Converters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace McWuT.Web.Pages.Tools;

[AllowAnonymous]
public class CSharpJsonConverterModel(IJsonCSharpConversionService converter) : PageModel
{
    private readonly IJsonCSharpConversionService _converter = converter;

    [BindProperty]
    public string? Input { get; set; }

    [BindProperty]
    public string Mode { get; set; } = "auto"; // auto-detected on client

    [BindProperty]
    public bool IncludeJsonPropertyName { get; set; } = true;

    [BindProperty]
    public string RootClassName { get; set; } = "Root";

    [BindProperty]
    public string? TargetType { get; set; }

    [BindProperty]
    public string? Namespace { get; set; }

    [BindProperty]
    public bool UseFileScopedNamespace { get; set; }

    [BindProperty]
    public bool UseNullableContext { get; set; } = true;

    public string? Output { get; private set; }
    public string? Error { get; private set; }
    public IReadOnlyList<string>? TypeNames { get; private set; }

    public void OnGet()
    {
    }

    public IActionResult OnPostConvert()
    {
        try
        {
            var mode = DetectMode(Input ?? string.Empty);
            if (mode == "json-to-csharp")
            {
                var code = _converter.GenerateCSharpFromJson(Input ?? string.Empty, new JsonToCSharpOptions
                {
                    RootClassName = string.IsNullOrWhiteSpace(RootClassName) ? "Root" : RootClassName,
                    IncludeJsonPropertyNameAttributes = IncludeJsonPropertyName,
                    NamespaceName = string.IsNullOrWhiteSpace(Namespace) ? null : Namespace,
                    UseFileScopedNamespace = UseFileScopedNamespace,
                    UseNullableReferenceTypes = UseNullableContext
                });
                Output = code;
            }
            else
            {
                Output = _converter.GenerateSampleJsonFromCSharp(Input ?? string.Empty, string.IsNullOrWhiteSpace(TargetType) ? null : TargetType);
            }
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }

        return Page();
    }

    public IActionResult OnPostDiscover()
    {
        try
        {
            TypeNames = _converter.DiscoverTypeNames(Input ?? string.Empty);
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        return Page();
    }

    private static string DetectMode(string text)
    {
        var trimmed = text.TrimStart();
        if (trimmed.StartsWith("{")) return "json-to-csharp";
        if (trimmed.StartsWith("[")) return "json-to-csharp";
        if (trimmed.Contains(" class ", StringComparison.Ordinal) ||
            trimmed.Contains(" struct ", StringComparison.Ordinal) ||
            trimmed.Contains(" record ", StringComparison.Ordinal) ||
            trimmed.StartsWith("public") || trimmed.StartsWith("internal") || trimmed.StartsWith("private"))
        {
            return "csharp-to-json";
        }
        return "json-to-csharp";
    }
}
