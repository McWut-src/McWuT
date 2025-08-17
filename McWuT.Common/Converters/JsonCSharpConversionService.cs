using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace McWuT.Common.Converters;

public class JsonCSharpConversionService : IJsonCSharpConversionService
{
    private static readonly JsonSerializerOptions Indented = new(JsonSerializerDefaults.General)
    {
        WriteIndented = true
    };

    private static readonly HashSet<string> ValueTypes = new(StringComparer.Ordinal)
    {
        "bool","int","long","double","decimal","DateTime","Guid"
    };

    public string GenerateCSharpFromJson(string json, string rootClassName = "Root")
        => GenerateCSharpFromJson(json, new JsonToCSharpOptions { RootClassName = rootClassName });

    public string GenerateCSharpFromJson(string json, JsonToCSharpOptions options)
    {
        if (string.IsNullOrWhiteSpace(json)) throw new ArgumentException("JSON is empty");
        JsonNode? node;
        try { node = JsonNode.Parse(json); }
        catch (Exception ex) { throw new ArgumentException($"Invalid JSON: {ex.Message}"); }
        if (node is null) throw new ArgumentException("Invalid JSON");

        var classes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var sb = new StringBuilder();

        string rootName = MakeValidIdentifier(ToPascalCase(options.RootClassName));
        BuildClassForNode(node, rootName, classes, options);

        // Header
        if (options.UseNullableReferenceTypes) sb.AppendLine("#nullable enable");
        if (!string.IsNullOrWhiteSpace(options.NamespaceName))
        {
            if (options.UseFileScopedNamespace)
            {
                sb.AppendLine($"namespace {options.NamespaceName};");
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine($"namespace {options.NamespaceName}");
                sb.AppendLine("{");
            }
        }

        foreach (var kvp in classes)
        {
            sb.AppendLine($"public class {kvp.Key}");
            sb.AppendLine("{");
            sb.Append(kvp.Value);
            sb.AppendLine("}");
            sb.AppendLine();
        }

        if (!string.IsNullOrWhiteSpace(options.NamespaceName) && !options.UseFileScopedNamespace)
        {
            sb.AppendLine("}");
        }

        return sb.ToString();
    }

    private static void BuildClassForNode(JsonNode node, string className, Dictionary<string, string> classes, JsonToCSharpOptions options)
    {
        if (classes.ContainsKey(className)) return;

        var propSb = new StringBuilder();
        if (node is JsonObject obj)
        {
            foreach (var kv in obj)
            {
                var jsonKey = kv.Key;
                var propName = MakeValidIdentifier(ToPascalCase(jsonKey));
                string typeName = GetTypeForNode(kv.Value, propName, classes, options);

                if (options.IncludeJsonPropertyNameAttributes && propName != jsonKey)
                {
                    propSb.AppendLine($"    [System.Text.Json.Serialization.JsonPropertyName(\"{jsonKey}\")] ");
                }

                // Nullability handling
                bool isList = typeName.StartsWith("List<", StringComparison.Ordinal);
                bool isValueType = ValueTypes.Contains(typeName);
                bool jsonIsNull = kv.Value == null; // explicit null in JSON

                if (jsonIsNull)
                {
                    // make the type nullable if possible
                    if (isValueType || isList)
                    {
                        // value types and List<T> use nullable suffix on the type itself
                        typeName = typeName + "?";
                    }
                    else
                    {
                        // class/string -> nullable reference
                        typeName = typeName + "?";
                    }
                }
                else if (options.UseNullableReferenceTypes && !isValueType && !isList)
                {
                    // In #nullable enable, make reference types nullable by default to avoid warnings
                    typeName = typeName + "?";
                }

                propSb.AppendLine($"    public {typeName} {propName} {{ get; set; }}");
            }
        }
        else if (node is JsonArray arr)
        {
            string itemType = "object";
            if (arr.Count > 0)
            {
                itemType = GetTypeForNode(arr[0], className + "Item", classes, options);
            }
            propSb.AppendLine($"    public List<{itemType}> Items {{ get; set; }} = new();");
        }
        else
        {
            string typeName = GetPrimitiveType((JsonValue?)node);
            propSb.AppendLine($"    public {typeName} Value {{ get; set; }}");
        }

        classes[className] = propSb.ToString();
    }

    private static string GetTypeForNode(JsonNode? node, string hintName, Dictionary<string, string> classes, JsonToCSharpOptions options)
    {
        if (node is null) return "object?";
        switch (node)
        {
            case JsonObject obj:
                string className = hintName.EndsWith("Item", StringComparison.Ordinal) ? hintName : hintName + "Model";
                BuildClassForNode(obj, className, classes, options);
                return className;
            case JsonArray arr:
                if (arr.Count == 0) return "List<object?>?";
                var first = arr[0];
                string elemType = GetTypeForNode(first, hintName + "Item", classes, options);
                return $"List<{elemType}>";
            case JsonValue val:
                return GetPrimitiveType(val);
            default:
                return "object";
        }
    }

    private static string GetPrimitiveType(JsonValue? val)
    {
        if (val is null) return "object?";
        if (val.TryGetValue(out string? _)) return "string";
        if (val.TryGetValue(out bool _)) return "bool";
        if (val.TryGetValue(out int _)) return "int";
        if (val.TryGetValue(out long _)) return "long";
        if (val.TryGetValue(out double _)) return "double";
        if (val.TryGetValue(out decimal _)) return "decimal";
        if (val.TryGetValue(out DateTime _)) return "DateTime";
        if (val.TryGetValue(out Guid _)) return "Guid";
        return "string";
    }

    public string GenerateSampleJsonFromCSharp(string csharp)
        => GenerateSampleJsonFromCSharp(csharp, null);

    public string GenerateSampleJsonFromCSharp(string csharp, string? targetTypeName)
    {
        if (string.IsNullOrWhiteSpace(csharp)) throw new ArgumentException("C# is empty");
        try
        {
            var types = ParseClasses(csharp);
            if (types.Count == 0) throw new Exception("No class definition found");

            var selected = targetTypeName != null
                ? types.FirstOrDefault(t => string.Equals(t.Name, targetTypeName, StringComparison.Ordinal))
                : types[0];

            if (selected == null) throw new Exception($"Type '{targetTypeName}' not found");

            var node = BuildSampleForClass(selected, types);
            return node.ToJsonString(Indented);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Invalid C# class: {ex.Message}");
        }
    }

    public IReadOnlyList<string> DiscoverTypeNames(string csharp)
    {
        var classes = ParseClasses(csharp);
        return classes.Select(c => c.Name).ToList();
    }

    private sealed record PropertyInfo(string Name, string Type, bool IsList, bool IsNullable);
    private sealed record ClassInfo(string Name, List<PropertyInfo> Properties);

    private static List<ClassInfo> ParseClasses(string code)
    {
        var list = new List<ClassInfo>();
        foreach (Match classMatch in Regex.Matches(code, @"\b(class|record|struct)\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)"))
        {
            var className = classMatch.Groups["name"].Value;
            var props = new List<PropertyInfo>();
            var propRegex = new Regex(@"public\s+(?:(?:virtual|static|readonly|required|sealed|new|partial)\s+)*" +
                                      @"(?<type>[\w\.<>\[\]\?]+)\s+" +
                                      @"(?<name>[A-Za-z_][A-Za-z0-9_]*)\s*" +
                                      @"\{\s*get\s*;\s*(?:set|init)\s*;\s*\}\s*(?:=\s*[^;]+;)?",
                                      RegexOptions.Multiline);
            foreach (Match m in propRegex.Matches(code))
            {
                var type = m.Groups["type"].Value.Trim();
                var name = m.Groups["name"].Value.Trim();
                bool isNullable = type.EndsWith("?");
                string coreType = type.TrimEnd('?');
                bool isList = TryExtractEnumerableElementType(coreType, out var elementType);
                string finalType = isList ? elementType : coreType;
                props.Add(new PropertyInfo(name, finalType, isList, isNullable));
            }
            list.Add(new ClassInfo(className, props));
        }
        return list;
    }

    private static bool TryExtractEnumerableElementType(string type, out string elementType)
    {
        var m = Regex.Match(type, @"^(?:[A-Za-z_][A-Za-z0-9_\.]*\.)?(?:List|IList|ICollection|IEnumerable|ObservableCollection)<(?<elem>.+)>$");
        if (m.Success)
        {
            elementType = m.Groups["elem"].Value.Trim();
            return true;
        }
        elementType = type;
        return false;
    }

    private static JsonNode BuildSampleForClass(ClassInfo cls, List<ClassInfo> all)
    {
        var obj = new JsonObject();
        foreach (var p in cls.Properties)
        {
            JsonNode sample = BuildSampleForType(p.Type, p.IsNullable, all);
            if (p.IsList)
            {
                var arr = new JsonArray();
                arr.Add(sample);
                obj[p.Name] = arr;
            }
            else
            {
                obj[p.Name] = sample;
            }
        }
        return obj;
    }

    private static JsonNode BuildSampleForType(string type, bool nullable, List<ClassInfo> all)
    {
        if (nullable) return JsonValue.Create((string?)null)!;
        switch (type)
        {
            case "string": return JsonValue.Create("sample");
            case "int": return JsonValue.Create(123);
            case "long": return JsonValue.Create(1234567890L);
            case "double": return JsonValue.Create(123.45);
            case "decimal": return JsonValue.Create(123.45m);
            case "bool": return JsonValue.Create(true);
            case "DateTime": return JsonValue.Create(DateTime.UtcNow);
            case "Guid": return JsonValue.Create(Guid.Empty);
            default:
                var match = all.FirstOrDefault(c => c.Name == type);
                return match != null ? BuildSampleForClass(match, all) : new JsonObject();
        }
    }

    private static string ToPascalCase(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return s;
        var parts = Regex.Split(s, "[^A-Za-z0-9]+").Where(p => p.Length > 0).ToArray();
        return string.Concat(parts.Select(p => char.ToUpperInvariant(p[0]) + p.Substring(1)));
    }

    private static string MakeValidIdentifier(string s)
    {
        if (string.IsNullOrEmpty(s)) return "Property";
        s = Regex.Replace(s, "[^A-Za-z0-9_]", "");
        if (!char.IsLetter(s, 0) && s[0] != '_') s = "_" + s;
        return s;
    }
}
