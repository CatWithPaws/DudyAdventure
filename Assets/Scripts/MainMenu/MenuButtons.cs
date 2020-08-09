using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

    private string path;

    public static UnityEvent OnSavePanelAnimationFinished = new UnityEvent();
    public static UnityEvent OnSavePanelAnimationStarted = new UnityEvent();

    public static UnityEvent OnSettingPanelAnimationStarted = new UnityEvent();
    public static UnityEvent OnSettingPanelAnimationFinished = new UnityEvent();

    public static UnityEvent OnAnimationToLoadLevelFinished = new UnityEvent();

    public static UnityEvent OnSettingsChanged = new UnityEvent();

    private bool isOpenSavePanel = false, isOpenSettingsPanel = false,  canSwitchAnimationInSavePanel = true,canSwitchAnimationInSettings = true;
    private bool isSavePanelOnceOpened = false;
    [SerializeField] private Animator SavePanelAnimator;
    [SerializeField] private Animator SettingsAnimator;
    [Header("Save Panel")]
    [SerializeField] private GameObject[] SaveDatas;
    [SerializeField] private GameObject[] CreateNewGameTexts;
    [SerializeField] private GameObject[] DeleteButtons;
    [SerializeField] private GameObject[] SaveButtons;
    [Header("Settings")]
    [SerializeField] private Slider MusicVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    private int whichSaveWillLoad;
    [Header("Animation Objects")]
    [SerializeField] private Animation DudyAnimation;
    [Header("GameObjects")]
    [SerializeField] GameObject Menu;
    [SerializeField] GameObject SavePanel;
    [SerializeField] GameObject SettingsMenu;
    [Header("Audio")]
    [SerializeField] AudioClip MenuMusic;
    [SerializeField] AudioClip ForestMusic;
    private void Start()
    {

        

        OnSavePanelAnimationFinished.AddListener(EnableCanTransitionInSavePanel);
        OnSavePanelAnimationStarted.AddListener(DisableCanTransitionInSavePanel);

        OnSettingPanelAnimationFinished.AddListener(EnableCanTransitionInSettingsPanel);
        OnSettingPanelAnimationStarted.AddListener(DisableCanTransitionSettingsPanel);

        OnAnimationToLoadLevelFinished.AddListener(LoadGameFromSave);

        OnSettingsChanged.AddListener(MusicObject.Instance.ApplyMusic);

        path = Application.persistentDataPath + "/";
        FileManager.Instance.LoadSettings();
        MusicObject.Instance.audioSource.clip = MenuMusic;
        MusicObject.Instance.ApplyMusic();
        MusicObject.Instance.audioSource.Play();
        Time.timeScale = 1;

    }


    private void DisableCanTransitionInSavePanel()
    {
        canSwitchAnimationInSavePanel = false;
    }
    private void EnableCanTransitionInSavePanel()
    {
        canSwitchAnimationInSavePanel = true;
    }
    private void DisableCanTransitionSettingsPanel()
    {
        canSwitchAnimationInSettings = false;
    }
    private void EnableCanTransitionInSettingsPanel()
    {
        canSwitchAnimationInSettings = true;
    }

    public void ToogleSavePanel()
    {
        if (!canSwitchAnimationInSavePanel) return;
        if (isOpenSavePanel) { SavePanelAnimator.Play("DisappearSavePanel"); }
        else { SavePanelAnimator.Play("AppearSavePanel"); }
        isOpenSavePanel = !isOpenSavePanel;
        if (!isSavePanelOnceOpened) isSavePanelOnceOpened = true; CheckSaves();
    }

    public void LoadOldLevels()
	{
        SceneManager.LoadScene(3);
	}
    public void OnMusicVolumeValueChanged(Slider slider)
    {
        GlobalVars.Instance.SettingsData.MusicVolume = slider.value;
        OnSettingsChanged.Invoke();
    }
    public void OnSFXVolumeValueChanged(Slider slider)
    {
        GlobalVars.Instance.SettingsData.SFXVolume = slider.value;
        OnSettingsChanged.Invoke();
    }
    private void CheckSaves()
    {
        for (int i = 0; i < SaveButtons.Length; i++)
        {
            print(i);
            string savePath = path + "Save" + i.ToString() + ".sv";
            if (File.Exists(savePath))
            {
                MarkAsHasSave(i);
            }
            else
            {
                MarkAsCreateNewSave(i);
            }
        }
    }
    private void MarkAsCreateNewSave(int whichSave)
    {
        SaveDatas[whichSave].SetActive(false);
        CreateNewGameTexts[whichSave].SetActive(true);
        DeleteButtons[whichSave].SetActive(false);
        SaveButtons[whichSave].GetComponent<SaveButton>().SaveState = SaveButton.SaveStates.CreateNew;
    }
    //законы преобразования, композиции, декартовое произведение. дискретка
    private void MarkAsHasSave(int whichSave)
    {
        SaveDatas[whichSave].SetActive(true);
        CreateNewGameTexts[whichSave].SetActive(false);
        DeleteButtons[whichSave].SetActive(true);

        SaveData saveData = FileManager.Instance.GetSaveData(whichSave);
        SaveValues values = SaveDatas[whichSave].GetComponentInChildren<SaveValues>();
        TextAsset textAsset = Resources.Load<TextAsset>("LocationName");
        string[] names = textAsset.text.Split('/');
        string locationName = names[saveData.Level];
        values.Location.text = locationName;
        values.Deaths.text = saveData.Deaths.ToString();
        int playedMinutes = (saveData.PlayedTimeInSeconds / 60);
        int playedHours = (saveData.PlayedTimeInSeconds - playedMinutes * 60) / 60;
        int playedSeconds = saveData.PlayedTimeInSeconds %60;
        values.PlayingTimeInSeconds.text = playedHours.ToString() + " : " + playedMinutes.ToString() + " : " + playedSeconds.ToString();
    }

    public void DeleteSave(int whichSave)
    {
        FileManager.Instance.DeleteSave(whichSave);
        CheckSaves();
    } 
    public void OnSaveButtonPressed(int whichSave)
    {
        whichSaveWillLoad = whichSave;
        Menu.SetActive(false);
        SavePanel.SetActive(false);
        SettingsMenu.SetActive(false);
        DudyAnimation.Play();
    }
    public void LoadGameFromSave()
    {
        SaveButton.SaveStates saveState = SaveButtons[whichSaveWillLoad].GetComponent<SaveButton>().SaveState;
        string savePath = path + "Save" + whichSaveWillLoad.ToString() + ".sv";
        FileManager.Instance.LoadSettings();
        MusicObject.Instance.songName = ForestMusic.name;
        FileManager.Instance.LoadGame(savePath);
        
    }

    private void OnApplicationQuit()
    {
        FileManager.Instance.SaveGame();
        FileManager.Instance.SaveSettings();
    }

   public void ExitGame()
    {
        Application.Quit();
    }

    public void ToogleSettingsPanel()
    {
        if (!canSwitchAnimationInSettings) return;
        if (!isOpenSettingsPanel)
        {
            SettingsAnimator.Play("OpenSettingsPanel");
            FileManager.Instance.LoadSettings();
            MusicVolumeSlider.value = GlobalVars.Instance.SettingsData.MusicVolume;
            SFXVolumeSlider.value = GlobalVars.Instance.SettingsData.SFXVolume;
        }
        else
        {
            SettingsAnimator.Play("CloseSettingsPanel");
            FileManager.Instance.LoadSettings();
            OnSettingsChanged.Invoke();
        }
        isOpenSettingsPanel = !isOpenSettingsPanel;
    }

    public void OnApplySettingsButtonPressed() {
        FileManager.Instance.SaveSettings();
        OnSettingsChanged.Invoke();
    }
}
