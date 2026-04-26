using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;
    public MenuInputHandler pauseMenuInputHandler;
    [SerializeField] private PlayerController playerController;

    [Header("Input System")]
    [SerializeField] private InputActionReference pauseAction;

    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    void Update()
    {
        if (SceneLoader.Instance != null && SceneLoader.Instance.IsLoading) return;

        if (pauseAction.action.WasPressedThisFrame())
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        if (SceneLoader.Instance != null && SceneLoader.Instance.IsLoading) return;

        StartCoroutine(OpenPauseMenuNextFrame());

        Time.timeScale = 0f;
        AudioListener.pause = true;
        isPaused = true;

        if (CursorManager.Instance != null)
            CursorManager.Instance.SetMenuMode(true);

        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.canvasGroup.alpha = 0f;
            ScreenFader.Instance.canvasGroup.blocksRaycasts = false;
        }

        if (playerController != null)
            playerController.OnGamePaused();
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        isPaused = false;

        if (CursorManager.Instance != null)
            CursorManager.Instance.SetMenuMode(false);

        if (playerController != null)
            playerController.OnGameResumed();
    }

    public void GoToMainMenu()
    {
        if (CursorManager.Instance != null)
            CursorManager.Instance.SetMenuMode(true);

        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadScene("MainMenu");

    }

    //this ensures that there is no issue with input bleed for first selected button
    private IEnumerator OpenPauseMenuNextFrame()
    {
        if (pauseMenuInputHandler != null)
            pauseMenuInputHandler.ClearSelection();

        //wait for 1 frame
        yield return null;

        pauseMenu.SetActive(true);

        //wait for 1 frame
        yield return null;

        if (pauseMenuInputHandler != null)
            pauseMenuInputHandler.ClearSelection();
    }

    private void OnEnable()
    {
        pauseAction.action.Enable();
    }

    private void OnDisable()
    {
        pauseAction.action.Disable();
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
