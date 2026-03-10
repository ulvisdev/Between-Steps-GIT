using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public CanvasGroup mainMenu;
    public CanvasGroup optionsMenu;

    public float transitionDuration = 0.25f;

    void Start()
    {
        ShowMainMenuInstant();
    }

    // ---------- BUTTON EVENTS ----------

    public void OpenOptionsMenu()
    {
        StartCoroutine(TransitionMenus(mainMenu, optionsMenu));
    }

    public void BackToMainMenu()
    {
        StartCoroutine(TransitionMenus(optionsMenu, mainMenu));
    }

    // ---------- INITIAL STATE ----------

    void ShowMainMenuInstant()
    {
        mainMenu.alpha = 1;
        mainMenu.interactable = true;
        mainMenu.blocksRaycasts = true;

        optionsMenu.alpha = 0;
        optionsMenu.interactable = false;
        optionsMenu.blocksRaycasts = false;
        optionsMenu.gameObject.SetActive(false);
    }

    // ---------- TRANSITION SYSTEM ----------

    IEnumerator TransitionMenus(CanvasGroup currentMenu, CanvasGroup nextMenu)
    {
        float timer = 0f;

        nextMenu.gameObject.SetActive(true);

        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / transitionDuration;

            // fade out current
            currentMenu.alpha = 1 - progress;

            // fade in next
            nextMenu.alpha = progress;

            yield return null;
        }

        // disable the old menu
        currentMenu.interactable = false;
        currentMenu.blocksRaycasts = false;
        currentMenu.gameObject.SetActive(false);

        // enable the new menu
        nextMenu.interactable = true;
        nextMenu.blocksRaycasts = true;
    }
}
