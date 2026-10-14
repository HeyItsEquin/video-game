using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager Instance;
    private Dictionary<string, Option> Options;

    void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }

    
}

public class Option
{
    private string name;
    private object value;

    string GetOptName()
    {
        return name;
    }

    T GetValue<T>()
    {
        if (value is T t)
            return t;

        try
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch (InvalidCastException)
        {
            return default;
        }
    }

    void SetValue<T>(T newVal)
    {
        if (value is T)
        {
            value = newVal;
            return;
        }

        throw new ArgumentException($"Passed invalid type to SetValue of option {name}. Expected: {value.GetType().Name}, Got: {newVal.GetType().Name}");
    }
}