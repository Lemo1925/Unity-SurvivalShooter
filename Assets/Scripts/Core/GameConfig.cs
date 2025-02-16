using System.IO;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    private static string configFilePath;
    public static Config config;
  
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        configFilePath = Path.Combine(Application.dataPath, "config.json");

        if (File.Exists(configFilePath)) 
        {
            using (FileStream fs = new FileStream(configFilePath, FileMode.Open))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Config));
                config = (Config)serializer.ReadObject(fs);
            }
        }
        else
        {
            config = new Config();
            using (FileStream fs = new FileStream(configFilePath, FileMode.Create))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Config));
                serializer.WriteObject(fs, config);
            }
        }
    }

    public static void UpdateConfig(float MusicVolume, float EffectVolume)
    {
        config.MusicVolume = MusicVolume;
        config.EffectVolume = EffectVolume;
    }

    public static void SaveConfig()
    {
        using (FileStream fs = new FileStream(configFilePath, FileMode.Create))
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Config));
            serializer.WriteObject(fs, config);
        }
    }
}
