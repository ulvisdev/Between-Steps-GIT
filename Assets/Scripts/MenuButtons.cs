using UnityEngine;

public class MenuButtonAction : MonoBehaviour
{
    [SerializeField] private UIPunch punch;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private PauseMenuManager pauseMenuManager;
    [SerializeField] private string sceneName;

    public void NewGame()
    {
        punch.PlayThen(() => menuManager.NewGame(sceneName));
    }

    public void ContinueGame()
    {
        punch.PlayThen(() => menuManager.ContinueGame());
    }

    public void LoadLevelFresh()
    {
        punch.PlayThen(() => menuManager.LoadLevelFresh(sceneName));
    }

    public void ShowOptionsMenu()
    {
        punch.PlayThen(() => menuManager.ShowOptionsMenu());
    }

    public void ShowLevelMenu()
    {
        punch.PlayThen(() => menuManager.ShowLevelMenu());
    }

    public void ShowMainMenuFromOptions()
    {
        punch.PlayThen(() => menuManager.ShowMainMenuFromOptions());
    }

    public void ShowMainMenuFromLevel()
    {
        punch.PlayThen(() => menuManager.ShowMainMenuFromLevel());
    }

    public void QuitGame()
    {
        punch.PlayThen(() => menuManager.QuitGame());
    }

    public void ResumeGame()
    {
        punch.PlayThen(() => pauseMenuManager.ResumeGame());
    }

    public void GoToMainMenu()
    {
        punch.PlayThen(() => pauseMenuManager.GoToMainMenu());
    }

    public void QuitGameFromPause()
    {
        punch.PlayThen(() => pauseMenuManager.QuitGame());
    }

}