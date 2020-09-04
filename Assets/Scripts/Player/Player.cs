using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(PlayerMoveComponent))]
[RequireComponent(typeof(PlayerAnimationComponent))]



public class Player : MonoBehaviour
{
    

    public enum State
    {
        IDLE,
        WALK,
        JUMP,
        FALL,
        DASH,
        WALL_JUMP,
        SLIDE_ON_WALL,
        DIALOG
    }
    public static Player Instance;
    [SerializeField] private PlayerMoveComponent playerMove;
    [SerializeField] private PlayerAnimationComponent playerAnimation;
    [SerializeField] private GameObject PressFText;
    [SerializeField] private GameObject SpawnPoint;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private AudioSource Music;
    private bool isInPause =  false;
    [SerializeField]
    private State currentState;
    public State CurrentState
    {
        get
        {
            return currentState;
        }
        private set
        {
            currentState = value;
        }
    }
    public  float directionByX;
    private void Awake()
    {
        if (!Instance) Instance = this;
        else if (Instance != this) Destroy(gameObject);
        EventHolder.OnPlayerDeadEvent.AddListener(OnPlayerDead);
        EventHolder.OnGUIDialogStarted.AddListener(DisablePressFText);
        EventHolder.OnGUIDialogEnded.AddListener(EnablePressFText);
        playerAnimation = GetComponent<PlayerAnimationComponent>();
        playerMove = GetComponent<PlayerMoveComponent>();
        StopCoroutine(Timer());
        StartCoroutine(Timer());
        ApplyAudio();
        FileManager.Instance?.LoadSettings();
        if (MusicObject.Instance)
        {
            AudioSource audioSource = MusicObject.Instance.audioSource;
            audioSource.volume = GlobalVars.Instance.GetSettingData().MusicVolume;
            MusicObject.Instance?.PlayMusic();
        }
        
    }
    public bool CanBGMove()
	{
        return playerMove.CanBGMove();
	}
    private void Update()
    {
        playerMove.CheckInput();
        playerMove.UpdateCheckDependencies(ref directionByX);
        if (Input.GetKeyDown(KeyCode.Escape)) {
           
            TogglePause();
        }
        Time.timeScale = isInPause ? 0 : 1;

       
        playerMove.CatchMove();
       // print(currentState);
    }

    private void TogglePause()
    {
        isInPause = !isInPause;
        if (isInPause)
        {
            FileManager.Instance.LoadSettings();
            PauseMenu pauseMenu = PauseMenu.GetComponent<PauseMenu>();
            pauseMenu.MusicVolumeSlider.value = GlobalVars.Instance.SettingsData.MusicVolume;
            pauseMenu.SFXVolumeSlider.value = GlobalVars.Instance.SettingsData.SFXVolume;
        }
        PauseMenu.SetActive(isInPause);

    }

    
    private void FixedUpdate()
    {
        playerMove.MovePlayer(out currentState,ref directionByX);
    }
    private void LateUpdate()
    {
        playerAnimation.AnimatePlayer(ref currentState,ref directionByX);
    }

    private void OnPlayerDead()
    {
        gameObject.transform.position = SpawnPoint.transform.position;
        if (!GlobalVars.Instance) return;
        GlobalVars.Instance.SaveData.Deaths++;
        FileManager.Instance.SaveGame();
    }
    private void DisablePressFText()
    {
        SetActivePressFButton(false);
    }
    private void EnablePressFText()
    {
        SetActivePressFButton(true);
    }
    public void SetActivePressFButton(bool value)
    {
        PressFText.SetActive(value);
    }

    public void RestoreCanTeleport()
    {
        playerMove.RestoreTeleport();
    }
    public bool CanTeleport()
    {
        return playerMove.canTeleport;
    }
    private IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            GlobalVars.Instance?.IncreasePlayedTime();
        }
    }
    private void StopTime() => Time.timeScale = 0;
    private void ResumeTime() => Time.timeScale = 1;
    
    public void ReturnToMenu() {
        ResumeTime();
        SceneManager.LoadScene("Menu");
    }
    public void ResumeGame()
    {
        ResumeTime();
        TogglePause();
    }

    public void OnMusicSliderChanged(UnityEngine.UI.Slider slider)
    {
        GlobalVars.Instance.SettingsData.MusicVolume = slider.value;
        FileManager.Instance.SaveSettings();
        ApplyAudio();
    }
    public void OnSFXSliderChanged(UnityEngine.UI.Slider slider)
    {
        GlobalVars.Instance.SettingsData.SFXVolume = slider.value;
        FileManager.Instance.SaveSettings();
        ApplyAudio();
    }

    public void ApplyAudio()
    {
        if (MusicObject.Instance == null) return;
        MusicObject.Instance.audioSource.volume = GlobalVars.Instance.SettingsData.MusicVolume;
    }

}
