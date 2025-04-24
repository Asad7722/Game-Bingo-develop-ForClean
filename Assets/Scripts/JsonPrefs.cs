namespace Games.Bingo
{
    using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
public static class JsonPrefs
{
    private static Dictionary<string, object> preferences = new Dictionary<string, object>();
    private static string filePath;
    static JsonPrefs()
    {
        filePath = Path.Combine(Application.persistentDataPath, "preferences.json");
        LoadPreferences();
    }
    public static string GetFilePath()
    {
        return filePath;
    }
    public static void LoadPreferences()
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                preferences = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }
            else
            {
                preferences = new Dictionary<string, object>();
            }
        }
        catch (Exception e)
        {
        }
    }
    private static void SavePreferences()
    {
        try
        {
            string updatedJson = JsonConvert.SerializeObject(preferences);
            File.WriteAllText(filePath, updatedJson);
        }
        catch (Exception e)
        {
        }
    }
    public static void Save()
    {
        SavePreferences();
    }
    public static void SetInt(string key, int value)
    {
        if (preferences.ContainsKey(key))
        {
            int currentValue = GetInt(key);
            preferences[key] = value;
        }
        else
        {
            preferences.Add(key, value);
        }
        SavePreferences();
    }
    public static int GetInt(string key, int defaultValue = 0)
    {
        if (preferences.TryGetValue(key, out object value))
        {
            if (value is int intValue)
            {  return intValue;
            }
            else if (value is long longValue)
            {
                int intValueFromLong = (int)longValue;
                return intValueFromLong;
            }
            else
            {
              }
        }
        else
        {
         }
        return defaultValue;
    }
    public static void SetFloat(string key, float value)
    {
        if (preferences.ContainsKey(key))
        {
            preferences[key] = value;
        }
        else
        {
            preferences.Add(key, value);
        }
        SavePreferences();
    }
    public static float GetFloat(string key, float defaultValue = 0f)
    {
        if (preferences.TryGetValue(key, out object value))
        {
            if (value is float floatValue)
            {
                return floatValue;
            }
            else if (value is double doubleValue)
            {
                return (float)doubleValue;
            }
            else if (value is int intValue)
            {
                return (float)intValue;
            }
            else
            {
                 Debug.LogWarning($"Key '{key}' found, but value type is not supported (Type: {value.GetType()}). Returning default value: {defaultValue}");
            }
        }
        else
        {  Debug.LogWarning($"Key '{key}' not found. Returning default value: {defaultValue}");
        }
        return defaultValue;
    }
    public static void SetString(string key, string value)
    {
        if (preferences.ContainsKey(key))
        {
            preferences[key] = value;
        }
        else
        {
            preferences.Add(key, value);
        }
        SavePreferences();
    }
    public static string GetString(string key, string defaultValue = "")
    {
        if (preferences.TryGetValue(key, out object value) && value is string stringValue)
        {
            return stringValue;
        }
        return defaultValue;
    }
    public static void SetBool(string key, bool value)
    {
        if (preferences.ContainsKey(key))
        {
            preferences[key] = value;
        }
        else
        {
            preferences.Add(key, value);
        }
        SavePreferences();
    }
    public static bool GetBool(string key, bool defaultValue = false)
    {
        if (preferences.TryGetValue(key, out object value) && value is bool boolValue)
        {
            return boolValue;
        }
        return defaultValue;
    }
    public static bool HasKey(string key)
    {
        return preferences.ContainsKey(key);
    }
    public static void DeleteKey(string key)
    {
        if (preferences.ContainsKey(key))
        {
            preferences.Remove(key);
            SavePreferences();
        }
    }
    public static void DeleteAll()
    {
        preferences.Clear();
        SavePreferences();
    }
}
}