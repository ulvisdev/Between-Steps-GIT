using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{

    public CanvasGroup mainMenu;
    public CanvasGroup optionsMenu;
    //private bool isInTransition;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
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
            optionsMenu.DOFade(1f, 0.5f).SetDelay(0.25f);
        });

        /*if (isInTransition) return;
        isInTransition = true;

        optionsMenu.gameObject.SetActive(true);
        mainMenu.DOFade(0f, 0.25f);
        optionsMenu.DOFade(1f, 0.25f).OnComplete(() => { mainMenu.gameObject.SetActive(false); 
                                                        isInTransition = false; });*/
    }

    public void ShowMainMenu()
    {

        //optionsMenu.DOFade(0f, 0.6f).OnComplete(() => { optionsMenu.gameObject.SetActive(false); });
        //mainMenu.DOFade(1f, 0.6f).OnComplete(() => { mainMenu.gameObject.SetActive(true); });

        optionsMenu.DOFade(0f, 0.5f).OnComplete(() =>
        {
            optionsMenu.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
            mainMenu.alpha = 0f;
            mainMenu.DOFade(1f, 0.5f).SetDelay(0.25f);
        });

        /*if (isInTransition) return;
        isInTransition = true;

        mainMenu.gameObject.SetActive(true);
        optionsMenu.DOFade(0f, 0.25f);
        mainMenu.DOFade(1f, 0.25f).OnComplete(() => { optionsMenu.gameObject.SetActive(false);
                                                        isInTransition = false; });*/
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
