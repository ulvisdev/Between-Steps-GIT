using UnityEngine;
using DG.Tweening;

public class UIPunch : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Click()
    {
        if (rectTransform == null) return;

        rectTransform.DOKill();
        rectTransform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 8, 0.8f)
            .SetUpdate(true);
    }
}