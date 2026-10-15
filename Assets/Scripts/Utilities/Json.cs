using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class Json
{
    public static string Serialize<T>(T value)
    {
        return JsonConvert.SerializeObject(value);
    }

    public static T Deserialize<T>(string json, T fallback)
    {
        try { return JsonConvert.DeserializeObject<T>(json); }
        catch (Exception e) when (e is JsonException || e is FormatException ||
                                  e is InvalidCastException || e is OverflowException)
        {
            UnityEngine.Debug.LogError($"Failed to deserialize object as {typeof(T).Name}");
            return fallback;
        }
    }

    public static Dictionary<string, string> ParseFile(string text)
    {
        var result = new Dictionary<string, string>();
        foreach (var prop in JObject.Parse(text).Properties())
            result[prop.Name] = prop.Value.ToString(Formatting.None);
        return result;
    }

    public static string ComposeFile(Dictionary<string, string> entries)
    {
        var obj = new JObject();
        foreach (var kv in entries)
            obj[kv.Key] = JToken.Parse(kv.Value);
        return obj.ToString(Formatting.Indented);
    }
}