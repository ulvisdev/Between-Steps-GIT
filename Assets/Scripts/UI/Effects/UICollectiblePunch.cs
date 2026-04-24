using UnityEngine;
using DG.Tweening;
using TMPro;

public class UICollectiblePunch : MonoBehaviour
{
    [SerializeField] private RectTransform[] targets;

    public void Punch()
    {
        foreach (RectTransform target in targets)
        {
            if (target == null) continue;

            target.DOKill();
            target.DOPunchScale(Vector3.one * 0.3f, 0.4f, 12, 0.8f).SetUpdate(true);
            target.GetComponent<UnityEngine.UI.Graphic>()?.DOFade(0.6f, 0.1f).SetLoops(2, LoopType.Yoyo);

            TMP_Text tmp = target.GetComponent<TMP_Text>();
            if (tmp != null)
            {
                tmp.DOFade(0.6f, 0.1f).SetLoops(2, LoopType.Yoyo);
            }
        }
    }
}