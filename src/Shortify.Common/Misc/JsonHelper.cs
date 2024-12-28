using System.Text.Json;

namespace Shortify.Common.Misc;

public class JsonHelper
{
    private readonly JsonSerializerOptions? _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    
    public T? Deserialize<T>(string? json)
    {
        return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
    }
}