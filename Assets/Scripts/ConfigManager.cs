using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ConfigManager
{
    public static ConfigElement Config { get; set; }
     
    public static void SaveConfig()
    {
        string json = JsonUtility.ToJson(Config);
        File.WriteAllText(Application.persistentDataPath + "config.json", json);
    }
 
    public static void LoadConfig()
    {
        if (!File.Exists(Application.persistentDataPath + "config.json"))
            CreateDefaultFile(new ConfigElement());

        string savedJson = File.ReadAllText(Application.persistentDataPath + "config.json");
        Config = JsonUtility.FromJson<ConfigElement>(savedJson);
    }

    private static void CreateDefaultFile(ConfigElement defaultCfg)
    {
        defaultCfg.ips.Add("127.0.0.1");
        string json = JsonUtility.ToJson(defaultCfg);
        File.WriteAllText(Application.persistentDataPath + "config.json", json);
    }
}

[Serializable]
public class ConfigElement
{
    public List<string> ips = new List<string>();
}
