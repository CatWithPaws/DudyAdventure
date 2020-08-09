using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalVars : MonoBehaviour
{
    public static GlobalVars Instance;
    public SaveData SaveData;
    public Settings SettingsData;
    public string SavePath { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    public void LoadLevel()
    {
        if (SaveData == null) return;
        SceneManager.LoadScene("Level" + SaveData.Level.ToString());
    }
    public void SetSavePath(string path)
    {
        SavePath = path;
    }
    public Settings GetSettingData() => SettingsData;
    public void IncreasePlayedTime()
    {
        SaveData.PlayedTimeInSeconds++;
    }
}

public class SaveData
{
    public int Level;
    public int Deaths;
    public int PlayedTimeInSeconds;
}
