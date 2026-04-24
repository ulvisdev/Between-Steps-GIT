using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class EndScreenManager : MonoBehaviour
{
    [Header("Texts in order")]
    [SerializeField] private List<CanvasGroup> texts;

    [Header("Final Button")]
    [SerializeField] private CanvasGroup menuButton;

    [Header("Timing")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float delayBetween = 1f;

    void Start()
    {
        foreach (CanvasGroup text in texts)
        {
            text.alpha = 0f;
            text.transform.localScale = Vector3.one * 0.9f;
        }

        menuButton.alpha = 0f;
        menuButton.gameObject.SetActive(false);

        PlaySequence();

        // Ensure the cursor is visible and unlocked for the end screen
        Cursor.visible = false;

        CursorManager cursorManager = FindFirstObjectByType<CursorManager>();

        if (cursorManager != null)
        {
            cursorManager.SetMenuMode(true);
        }
    }

    void PlaySequence()
    {
        Sequence seq = DOTween.Sequence();

        foreach (CanvasGroup text in texts)
        {
            seq.Append(text.DOFade(1f, fadeDuration));
            seq.Join(text.transform.DOScale(1f, fadeDuration).From(0.9f));
            seq.AppendInterval(delayBetween);
        }

        seq.AppendCallback(() =>
        {
            menuButton.gameObject.SetActive(true);
            menuButton.transform.localScale = Vector3.one * 0.9f;
        });

        seq.Append(menuButton.DOFade(1f, fadeDuration));
        seq.Join(menuButton.transform.DOScale(1f, fadeDuration).From(0.9f));
    }
}