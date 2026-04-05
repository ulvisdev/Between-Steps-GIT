using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private bool isPaused = false;
    private bool isLoading = false;
    public bool IsLoading => isLoading;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void LoadScene(string sceneName)
    {
        if (isLoading) return;
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    public void NextLevel()
    {
        if (isLoading) return;
        StartCoroutine(NextLevelRoutine());
    }

    public void ReloadScene()
    {
        if (isLoading) return;
        StartCoroutine(ReloadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        isLoading = true;

        if (ScreenFader.Instance != null)
            yield return StartCoroutine(ScreenFader.Instance.FadeOut());

        StopAllMusic();

        Time.timeScale = 1f;
        AudioListener.pause = false;
        isPaused = false;

        yield return SceneManager.LoadSceneAsync(sceneName);

        PlaySceneMusic();

        //wait for 1 frame so dotween has time to collect itself between scenes
        yield return null;

        if (ScreenFader.Instance != null)
            yield return StartCoroutine(ScreenFader.Instance.FadeIn());

        //PlaySceneMusic();            

        isLoading = false;
    }

    private IEnumerator NextLevelRoutine()
    {
        isLoading = true;

        if (ScreenFader.Instance != null)
            yield return StartCoroutine(ScreenFader.Instance.FadeOut());

        StopAllMusic();

        Time.timeScale = 1f;
        AudioListener.pause = false;
        isPaused = false;

        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        PlaySceneMusic();

        //wait for 1 frame so dotween has time to collect itself between scenes
        yield return null;

        if (ScreenFader.Instance != null)
            yield return StartCoroutine(ScreenFader.Instance.FadeIn());

        //PlaySceneMusic();

        isLoading = false;
    }

    private IEnumerator ReloadSceneRoutine()
    {
        isLoading = true;

        if (ScreenFader.Instance != null)
            yield return StartCoroutine(ScreenFader.Instance.FadeOut());

        StopAllMusic();

        Time.timeScale = 1f;
        isPaused = false;

        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        PlaySceneMusic();

        //wait for 1 frame so dotween has time to collect itself between scenes
        yield return null;

        if (ScreenFader.Instance != null)
            yield return StartCoroutine(ScreenFader.Instance.FadeIn());

        //PlaySceneMusic();

        isLoading = false;
    }

    private void StopAllMusic()
    {
        if (MenuAudioManager.Instance != null)
            MenuAudioManager.Instance.StopMusic();

        if (AudioManager.Instance != null)
            AudioManager.Instance.StopMusic();
    }

    private void PlaySceneMusic()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "MainMenu")
        {
            if (MenuAudioManager.Instance != null && MenuAudioManager.Instance.backgroundMusic != null)
                MenuAudioManager.Instance.PlayMusic(MenuAudioManager.Instance.backgroundMusic);
        }
        else
        {
            if (AudioManager.Instance != null && AudioManager.Instance.backgroundMusic != null)
                AudioManager.Instance.PlayMusic(AudioManager.Instance.backgroundMusic);
        }
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        isPaused = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}