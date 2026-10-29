using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public enum OptionCategory
{
    Gameplay = 0,
    Video,
    Controls,
    Audio,
    Misc
}

[AttributeUsage(AttributeTargets.Field)]
public abstract class ControlAttribute : Attribute
{
    public readonly OptionCategory Category;
    public readonly string Label;
    
    protected ControlAttribute(OptionCategory category, string label)
    {
        Category = category;
        Label = label;
    }

    public abstract Type KeyType { get; }
    public abstract VisualElement Create(object boxedKey);
}

public class SliderIntAttribute : ControlAttribute
{
    readonly int min, max;
    public SliderIntAttribute(OptionCategory c, string label, int min, int max) : base(c,label)
    {
        this.min = min;
        this.max = max;
    }

    public override Type KeyType => typeof(OptionKey<int>);

    public override VisualElement Create(object boxedKey)
    {
        var key = (OptionKey<int>)boxedKey;
        var slider = new SliderInt(Label, min, max) { value = SaveManager.Instance.Get(key) };
        slider.RegisterValueChangedCallback(e => SaveManager.Instance.Set(key, e.newValue));
        return slider;
    }
}

public sealed class OptionKey<T>
{
    public readonly string Name;
    public readonly T Default;
    public event Action<T> Changed;

    public OptionKey(string name, T defaultVal) { Name = name; Default = defaultVal; }

    internal void RaiseChanged(T value) => Changed?.Invoke(value);
}

[Preserve] // Il2CPP
public static class Opts
{
    [SliderInt(OptionCategory.Controls, "Camera Sensitivity", 1, 10)]
    public static readonly OptionKey<int> CameraSensitivity = new("controls.sensitivity", 5);

    [SliderInt(OptionCategory.Video, "Max Framerate", 30, 240)]
    public static readonly OptionKey<int> MaxFramerate = new("video.maxFramerate", 30);

    public static readonly OptionKey<string> CurrentLevelId = new("player.currentLevel", "l0");
    public static readonly OptionKey<Checkpoint> PlayerLastCheckpoint = new("player.lastCheckpoint", null);
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    readonly Dictionary<string, string> rawOpts = new();

    readonly Dictionary<string, object> cache = new();

    bool dirty;
    string path;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Bootstrap()
    {
        var go = new GameObject("saveManager");
        go.AddComponent<SaveManager>();
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        path = Path.Combine(Application.persistentDataPath, "prefs.json");
        Load();
    }

    public void Apply<T>(OptionKey<T> key)
    {
        var val = Get(key);
        key.RaiseChanged(val);
    }

    public T Get<T>(OptionKey<T> key)
    {
        if (cache.TryGetValue(key.Name, out var c)) return (T)c;
        var v = rawOpts.TryGetValue(key.Name, out var json) ? Json.Deserialize(json, key.Default) : key.Default;
        cache[key.Name] = v;
        return v;
    }

    public void Set<T>(OptionKey<T> key, T value)
    {
        rawOpts[key.Name] = Json.Serialize(value);
        cache[key.Name] = value;
        dirty = true;
        key.RaiseChanged(value);
    }

    public void Remove<T>(OptionKey<T> key)
    {
        rawOpts.Remove(key.Name);
        cache.Remove(key.Name);
        dirty = true;
        key.RaiseChanged(key.Default);
    }

    void Load()
    {
        if (!File.Exists(path)) return;
        try
        {
            foreach (var kv in Json.ParseFile(File.ReadAllText(path)))
                rawOpts[kv.Key] = kv.Value;
        }
        catch (Newtonsoft.Json.JsonException)
        {
            File.Copy(path, path + ".corrupt", true);
            rawOpts.Clear();
        }
    }

    public void Save()
    {
        if (!dirty) return;
        string tmp = path + ".tmp";
        File.WriteAllText(tmp, Json.ComposeFile(rawOpts));
        if (File.Exists(path)) File.Delete(path);
        File.Move(tmp, path);
        dirty = false;
    }

    void OnApplicationQuit()
    {
        Save();
    }
}