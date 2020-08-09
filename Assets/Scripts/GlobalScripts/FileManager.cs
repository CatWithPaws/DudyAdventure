using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
public class FileManager : MonoBehaviour
{
    public static FileManager Instance;
    public  string settingsPath { get; private set; }
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetSettingsPath(Application.persistentDataPath);
    }

    public  void SaveGame()
    {
        SaveData data = GlobalVars.Instance.SaveData;
        string path = GlobalVars.Instance.SavePath;
        using(StreamWriter streamWriter = new StreamWriter(path))
        {
            streamWriter.Write(JsonConvert.SerializeObject(data));
        }
    }
    public  void SetSettingsPath(string path)
    {
        settingsPath = path + "/Settings.stg";
    }
    public  void LoadGame(string path)
    {
        SaveData data;
        if (File.Exists(path))
        {
            using(StreamReader streamReader = new StreamReader(path))
            {
                data = JsonConvert.DeserializeObject<SaveData>(streamReader.ReadToEnd());
            }
        }
        else
        {
            using(StreamWriter streamWriter = new StreamWriter(path))
            {
                data = LoadDefaultSaveData();
                streamWriter.Write(JsonConvert.SerializeObject(data));
            }
        }

        GlobalVars.Instance.SaveData = data;
        GlobalVars.Instance.SetSavePath(path);
        FileManager.Instance.SaveGame();
        GlobalVars.Instance.LoadLevel();
    }

    public  void LoadSettings()
    {
        Settings settingsData = new Settings();

        if (!File.Exists(settingsPath)) MakeDefaultSettings(ref settingsData);
        else  LoadSettingsFromFile(ref settingsData);
        GlobalVars.Instance.SettingsData = settingsData;
        SaveSettings();
    }

    private  void MakeDefaultSettings(ref Settings settingsData)
    {
        settingsData.MusicVolume = 1f;
        settingsData.SFXVolume = 1f;
       
    }

    public void SaveSettings()
    {
        using (StreamWriter streamWriter = new StreamWriter(settingsPath))
        {
            string jsonData = JsonConvert.SerializeObject(GlobalVars.Instance.SettingsData);
            streamWriter.Write(jsonData);
        }
    }

    private  void LoadSettingsFromFile(ref Settings settingsData)
    {
        using (StreamReader streamReader = new StreamReader(settingsPath))
        {
            string jsonData = streamReader.ReadToEnd();
            settingsData = JsonConvert.DeserializeObject<Settings>(jsonData);
        }
    }

    private  string MakeStringPathFromSaveNum(int whichSave) => Application.persistentDataPath + "/Save" + whichSave.ToString() + ".sv";

    public  void DeleteSave(int whichSave)
    {
        string savePath = MakeStringPathFromSaveNum(whichSave);
        if (File.Exists(savePath)) File.Delete(savePath);

    }
    public  SaveData GetSaveData(int whichSave) 
    {
        string path = Application.persistentDataPath + "/Save" + whichSave.ToString() + ".sv";
        SaveData saveData;
        using (StreamReader streamReader = new StreamReader(path))
        {
            saveData = JsonConvert.DeserializeObject<SaveData>(streamReader.ReadToEnd());
        }
        return saveData;
    }
    private  SaveData LoadDefaultSaveData()
    {
        SaveData data = new SaveData();
        data.Deaths = 0;
        data.PlayedTimeInSeconds = 0;
        data.Level = 0;
        return data;
    }

}

public class Settings
{
    public float MusicVolume,SFXVolume;
}
