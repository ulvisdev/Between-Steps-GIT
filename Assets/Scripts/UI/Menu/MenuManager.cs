using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{

    public CanvasGroup mainMenu;
    public CanvasGroup optionsMenu;
    public CanvasGroup controlsMenu;
    public CanvasGroup musicMenu;
    public CanvasGroup levelMenu;

    //private bool isInTransition;
    public Button loadgameButton;
    public Button continueButton;
    public MenuInputHandler mainMenuInputHandler;
    public MenuInputHandler optionsMenuInputHandler;
    public MenuInputHandler levelMenuInputHandler;

    public MenuInputHandler controlsMenuInputHandler;
    public MenuInputHandler musicMenuInputHandler;

    public LevelButton[] levelButtons;

    [Header("Options")]
    [SerializeField] private Toggle speedrunnerModeToggle;

    void Start()
    {

        if (MenuAudioManager.Instance != null && MenuAudioManager.Instance.backgroundMusic != null)
        {
            MenuAudioManager.Instance.PlayMusic(MenuAudioManager.Instance.backgroundMusic);
        }

        if (ScreenFader.Instance != null)
        {
            //ScreenFader.Instance.canvasGroup.alpha = 0f;
            ScreenFader.Instance.canvasGroup.blocksRaycasts = false;
        }

        if (speedrunnerModeToggle != null)
        {
            speedrunnerModeToggle.isOn = GameSettings.SpeedrunnerModeEnabled;
            speedrunnerModeToggle.onValueChanged.AddListener(OnSpeedrunnerModeChanged);
        }

        UpdateLoadGameButton();
        UpdateContinueButton();

        if (mainMenuInputHandler != null)
            mainMenuInputHandler.ClearSelection();

        CursorManager.Instance.SetMenuMode(true);
        CursorManager.Instance.ShowCursor();

    }

    public void NewGame(string firstSceneName)
    {
        SaveSystem.ResetGameProgress();
        PlayerPrefs.DeleteKey(SaveSystem.GetLastSceneKey());
        RunState.ApplySettingsToNewRun();

        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadScene(firstSceneName);
    }

    public void ContinueGame()
    {
        string lastScene = PlayerPrefs.GetString(SaveSystem.GetLastSceneKey(), "");

        if (string.IsNullOrEmpty(lastScene))
        {
            Debug.Log("No continue data found.");
            return;
        }

        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadScene(lastScene);
    }

    public void LoadLevelFresh(string sceneName)
    {
        SaveSystem.ResetLevelRunProgress(sceneName);
        RunState.ApplySettingsToNewRun();

        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadScene(sceneName);
    }

    public void UpdateContinueButton()
    {
        if (continueButton == null)
            return;

        string lastScene = PlayerPrefs.GetString(SaveSystem.GetLastSceneKey(), "");
        continueButton.interactable = !string.IsNullOrEmpty(lastScene);
    }

    public void ShowLevelMenu()
    {

        RefreshLevelButtons();

        mainMenu.DOFade(0f, 0.5f).OnComplete(() =>
        {
            mainMenu.gameObject.SetActive(false);
            levelMenu.gameObject.SetActive(true);
            levelMenu.alpha = 0f;

            levelMenu.DOFade(1f, 0.5f).SetDelay(0.25f).OnComplete(() => { levelMenuInputHandler.ClearSelection(); });
        });
    }

    public void ShowOptionsMenu()
    {

        //mainMenu.DOFade(0f, 0.6f).OnComplete(() => { mainMenu.gameObject.SetActive(false); });
        //optionsMenu.DOFade(1f, 0.6f).OnComplete(() => { optionsMenu.gameObject.SetActive(true); });

        mainMenu.DOFade(0f, 0.5f).OnComplete(() =>
        {
            mainMenu.gameObject.SetActive(false);
            optionsMenu.gameObject.SetActive(true);
            optionsMenu.alpha = 0f;
            optionsMenu.DOFade(1f, 0.5f).SetDelay(0.25f).OnComplete(() => { optionsMenuInputHandler.ClearSelection(); });
        });

        /*if (isInTransition) return;
        isInTransition = true;

        optionsMenu.gameObject.SetActive(true);
        mainMenu.DOFade(0f, 0.25f);
        optionsMenu.DOFade(1f, 0.25f).OnComplete(() => { mainMenu.gameObject.SetActive(false); 
                                                        isInTransition = false; });*/
    }

    public void ShowMainMenuFromOptions()
    {

        //optionsMenu.DOFade(0f, 0.6f).OnComplete(() => { optionsMenu.gameObject.SetActive(false); });
        //mainMenu.DOFade(1f, 0.6f).OnComplete(() => { mainMenu.gameObject.SetActive(true); });

        optionsMenu.DOFade(0f, 0.5f).OnComplete(() =>
        {
            optionsMenu.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
            mainMenu.alpha = 0f;
            mainMenu.DOFade(1f, 0.5f).SetDelay(0.25f).OnComplete(() => { mainMenuInputHandler.ClearSelection(); });
        });

        /*if (isInTransition) return;
        isInTransition = true;

        mainMenu.gameObject.SetActive(true);
        optionsMenu.DOFade(0f, 0.25f);
        mainMenu.DOFade(1f, 0.25f).OnComplete(() => { optionsMenu.gameObject.SetActive(false);
                                                        isInTransition = false; });*/
    }

    public void ShowMainMenuFromLevel()
    {
        levelMenu.DOFade(0f, 0.5f).OnComplete(() =>
        {
            levelMenu.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
            mainMenu.alpha = 0f;
            mainMenu.DOFade(1f, 0.5f).SetDelay(0.25f).OnComplete(() => { mainMenuInputHandler.ClearSelection(); });
        });
    }

    public void ShowOptionsMenuFromControls()
    {
        controlsMenu.DOFade(0f, 0.5f).OnComplete(() =>
        {
            controlsMenu.gameObject.SetActive(false);
            optionsMenu.gameObject.SetActive(true);
            optionsMenu.alpha = 0f;
            optionsMenu.DOFade(1f, 0.5f).SetDelay(0.25f).OnComplete(() => { optionsMenuInputHandler.ClearSelection(); });
        });
    }

    public void ShowControlsMenu()
    {
        optionsMenu.DOFade(0f, 0.5f).OnComplete(() =>
        {
            optionsMenu.gameObject.SetActive(false);
            controlsMenu.gameObject.SetActive(true);
            controlsMenu.alpha = 0f;
            controlsMenu.DOFade(1f, 0.5f).SetDelay(0.25f).OnComplete(() => { controlsMenuInputHandler.ClearSelection(); });
        });
    }

    public void ShowOptionsMenuFromMusic()
    {
        musicMenu.DOFade(0f, 0.5f).OnComplete(() =>
        {
            musicMenu.gameObject.SetActive(false);
            optionsMenu.gameObject.SetActive(true);
            optionsMenu.alpha = 0f;
            optionsMenu.DOFade(1f, 0.5f).SetDelay(0.25f).OnComplete(() => { optionsMenuInputHandler.ClearSelection(); });
        });
    }

    public void ShowMusicMenu()
    {
        optionsMenu.DOFade(0f, 0.5f).OnComplete(() =>
        {
            optionsMenu.gameObject.SetActive(false);
            musicMenu.gameObject.SetActive(true);
            musicMenu.alpha = 0f;
            musicMenu.DOFade(1f, 0.5f).SetDelay(0.25f).OnComplete(() => { musicMenuInputHandler.ClearSelection(); });
        });
    }

    void RefreshLevelButtons()
    {
        int highestUnlocked = PlayerPrefs.GetInt("HighestLevel", 1);

        foreach (LevelButton levelButton in levelButtons)
        {
            if (levelButton != null)
                levelButton.Setup(highestUnlocked);
        }
    }

    public void HardReset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void UpdateLoadGameButton()
    {
        int highestLevel = PlayerPrefs.GetInt("HighestLevel", 1);

        loadgameButton.interactable = highestLevel >= 2;
    }

    private void OnSpeedrunnerModeChanged(bool isOn)
    {
        GameSettings.SpeedrunnerModeEnabled = isOn;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
