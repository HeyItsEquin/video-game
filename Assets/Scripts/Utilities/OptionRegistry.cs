using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UIElements;

public static class OptionRegistry
{
    public readonly struct Entry
    {
        public readonly string Label;
        readonly ControlAttribute attr; readonly object key;
        public Entry(ControlAttribute a, object k) { attr = a; key = k; Label = a.Label; }
        public VisualElement CreateControl() => attr.Create(key);
    }

    static readonly Dictionary<OptionCategory, List<Entry>> map = Build();

    public static IReadOnlyList<Entry> Get(OptionCategory c) =>
        map.TryGetValue(c, out var l) ? l : Array.Empty<Entry>();

    static Dictionary<OptionCategory, List<Entry>> Build()
    {
        var result = new Dictionary<OptionCategory, List<Entry>>();
        const BindingFlags F = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        foreach (var type in typeof(OptionRegistry).Assembly.GetTypes())
        foreach (var field in type.GetFields(F))
        {
            var attr = field.GetCustomAttribute<ControlAttribute>();
            if (attr == null) continue;

            if (field.FieldType != attr.KeyType)
            {
                UnityEngine.Debug.LogError($"{type.Name}.{field.Name}: {attr.GetType().Name} needs {attr.KeyType.Name}, field is {field.FieldType.Name}");
                continue;
            }

            if (!result.TryGetValue(attr.Category, out var list))
                result[attr.Category] = list = new();
            list.Add(new Entry(attr, field.GetValue(null)));
        }

        return result;
    }
}