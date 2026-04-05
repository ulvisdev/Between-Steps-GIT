using UnityEngine;
using DG.Tweening;
using System;

public class UIPunch : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool isPlaying;

    [SerializeField] private float punchAmount = 0.2f;
    [SerializeField] private float punchDuration = 0.2f;
    [SerializeField] private int vibrato = 8;
    [SerializeField] private float elasticity = 0.8f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void PlayThen(Action onComplete)
    {
        if (rectTransform == null)
        {
            onComplete?.Invoke();
            return;
        }

        if (isPlaying)
            return;

        isPlaying = true;

        rectTransform.DOKill();
        rectTransform.localScale = Vector3.one;

        rectTransform
            .DOPunchScale(Vector3.one * punchAmount, punchDuration, vibrato, elasticity)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                rectTransform.localScale = Vector3.one;
                isPlaying = false;
                onComplete?.Invoke();
            });
    }
}