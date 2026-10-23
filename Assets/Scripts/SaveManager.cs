using System.Collections.Generic;
using System.IO;
using UnityEngine;

public readonly struct OptionKey<T>
{
    public readonly string Name;
    public readonly T Default;
    public OptionKey(string name, T defaultVal) { Name = name; Default = defaultVal; }
}

public static class Opts
{
    public static readonly OptionKey<int> CameraSensitivity = new("controls.sensitivity", 5);
    public static readonly OptionKey<string> CurrentLevelId = new("player.currentLevel", "l0");
    public static readonly OptionKey<Checkpoint> PlayerLastCheckpoint = new("player.lastCheckpoint", null);
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    readonly Dictionary<string, string> rawOpts = new();

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

    public T Get<T>(OptionKey<T> key)
    {
        return rawOpts.TryGetValue(key.Name, out var json) ? Json.Deserialize(json, key.Default) : key.Default;
    }

    public void Set<T>(OptionKey<T> key, T value)
    {
        rawOpts[key.Name] = Json.Serialize(value);
        dirty = true;
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