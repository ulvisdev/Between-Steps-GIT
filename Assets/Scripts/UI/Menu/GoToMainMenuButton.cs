using UnityEngine;

public class GoToMainMenuButton : MonoBehaviour
{

    public static GoToMainMenuButton Instance { get; private set; }
    
    public void GoToMainMenu()
    {
        if (CursorManager.Instance != null)
            CursorManager.Instance.SetMenuMode(true);

        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadScene("MainMenu");
    }

}
