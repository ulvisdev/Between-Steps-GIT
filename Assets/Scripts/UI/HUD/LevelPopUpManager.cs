using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;
using UnityEngine.InputSystem;

public class LevelPopUpManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private CanvasGroup levelTextCanvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float minDisplayDuration = 0.5f;
    [SerializeField] private float maxWaitTime = 5f;

    private Coroutine currentPopUpRoutine;
    private bool hasPlayerInteracted;
    private Timer levelTimer;

    private void Start()
    {
        levelTimer = FindFirstObjectByType<Timer>();

        if (levelTextCanvasGroup != null)
            levelTextCanvasGroup.alpha = 0f;

        if (levelTimer != null)
            levelTimer.SetTimerActive(false);

        // ShowLevelText();
    }

    public void ShowLevelText()
    {

        if (currentPopUpRoutine != null)
            StopCoroutine(currentPopUpRoutine);

        hasPlayerInteracted = false;
        currentPopUpRoutine = StartCoroutine(ShowLevelPopUpRoutine());
    }

    private IEnumerator ShowLevelPopUpRoutine()
    {
        if (levelText == null || levelTextCanvasGroup == null)
        {
            Debug.LogWarning("LevelPopUpManager: Missing UI references.");
            yield break;
        }

        UpdateLevelText(SceneManager.GetActiveScene().buildIndex);

        // if (levelTimer != null)
        //     levelTimer.SetTimerActive(false);

        yield return levelTextCanvasGroup.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);

        float timer = 0f;
        while (timer < minDisplayDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        float waitTimer = 0f;
        while (!hasPlayerInteracted && waitTimer < maxWaitTime)
        {
            
            if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
                (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
                (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame))
            {
                hasPlayerInteracted = true;
            }

            waitTimer += Time.deltaTime;
            yield return null;
        }

        yield return levelTextCanvasGroup.DOFade(0f, fadeDuration).SetEase(Ease.InOutSine);

        if (GameSettings.SpeedrunnerModeEnabled && levelTimer != null)
            levelTimer.SetTimerActive(true);

        currentPopUpRoutine = null;
    }

    private void UpdateLevelText(int buildIndex)
    {
        levelText.text = $"Level {buildIndex}";
    }

    private void OnDestroy()
    {
        if (levelTextCanvasGroup != null)
            levelTextCanvasGroup.DOKill();
    }

}