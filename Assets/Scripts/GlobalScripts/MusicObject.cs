using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicObject : MonoBehaviour
{
    public static MusicObject Instance;
    public AudioSource audioSource;
    public string songName;
    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void PlayMusic()
    {
        if (audioSource == null) audioSource.Play();
        else if (audioSource.clip.name != songName)
        {
            audioSource.clip = Resources.Load("Music/" + songName) as AudioClip;
            audioSource.Play();
        }
    }
    public void ApplyMusic()
    {
        audioSource.volume = GlobalVars.Instance.SettingsData.MusicVolume;
    }
}
