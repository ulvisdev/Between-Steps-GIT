using UnityEngine;
using TMPro;
using DG.Tweening;

public class LevelPreviewUI : MonoBehaviour
{
    public static LevelPreviewUI Instance;

    [SerializeField] private GameObject panelRoot;
    [SerializeField] private CanvasGroup panelCanvasGroup;

    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private CanvasGroup levelNameCG;

    [SerializeField] private TextMeshProUGUI bestTimeText;
    [SerializeField] private CanvasGroup timeCG;

    [SerializeField] private TextMeshProUGUI deathsText;
    [SerializeField] private CanvasGroup deathsCG;

    [SerializeField] private TextMeshProUGUI bestCollectiblesText;
    [SerializeField] private CanvasGroup collectiblesCG;

    [SerializeField] private TextMeshProUGUI lockText;

    [Header("Collectibles Per Level")]
    [SerializeField] private int[] maxCollectiblesPerLevel;

    [Header("Animation")]
    [SerializeField] private float panelFadeDuration = 0.15f;
    [SerializeField] private float itemFadeDuration = 0.25f;
    [SerializeField] private float itemMoveDistance = 10f;
    [SerializeField] private float staggerDelay = 0.075f;

    private Sequence currentSequence;

    private RectTransform levelNameRect;
    private RectTransform timeRect;
    private RectTransform deathsRect;
    private RectTransform collectiblesRect;

    private Vector2 levelNameStartPos;
    private Vector2 timeStartPos;
    private Vector2 deathsStartPos;
    private Vector2 collectiblesStartPos;

    private void Awake()
    {
        Instance = this;

        levelNameRect = levelNameCG.GetComponent<RectTransform>();
        timeRect = timeCG.GetComponent<RectTransform>();
        deathsRect = deathsCG.GetComponent<RectTransform>();
        collectiblesRect = collectiblesCG.GetComponent<RectTransform>();

        levelNameStartPos = levelNameRect.anchoredPosition;
        timeStartPos = timeRect.anchoredPosition;
        deathsStartPos = deathsRect.anchoredPosition;
        collectiblesStartPos = collectiblesRect.anchoredPosition;

        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
            panelCanvasGroup.alpha = 0f;
        }

        ResetItemsInstant();
    }

    public void ShowPreview(int levelIndex, bool unlocked)
    {
        currentSequence?.Kill();
        panelCanvasGroup.DOKill();
        levelNameCG.DOKill();
        timeCG.DOKill();
        deathsCG.DOKill();
        collectiblesCG.DOKill();

        if (panelRoot != null)
            panelRoot.SetActive(true);

        ResetItemsInstant();

        string sceneName = "Level" + levelIndex;

        if (levelNameText != null)
            levelNameText.text = "Level " + levelIndex;

        int maxCollectibles = GetMaxCollectibles(levelIndex);

        if (!unlocked)
        {
            if (lockText != null)
                lockText.text = "Locked";

            if (bestTimeText != null)
                bestTimeText.text = "--:--.---";

            if (deathsText != null)
                deathsText.text = "-";

            if (bestCollectiblesText != null)
                bestCollectiblesText.text = "- / " + maxCollectibles;
        }
        else
        {
            if (lockText != null)
                lockText.text = "Unlocked";

            float bestTime = LevelStats.GetBestTime(sceneName);
            int totalDeaths = LevelStats.GetTotalDeaths(sceneName);
            int bestCollectibles = LevelStats.GetBestCollectibles(sceneName);

            if (bestTimeText != null)
                bestTimeText.text = bestTime >= 0f ? FormatTime(bestTime) : "--:--.---";

            if (deathsText != null)
                deathsText.text = totalDeaths.ToString();

            if (bestCollectiblesText != null)
                bestCollectiblesText.text = $"{bestCollectibles} / {maxCollectibles}";
        }

        currentSequence = DOTween.Sequence();

        currentSequence.Append(panelCanvasGroup.DOFade(1f, panelFadeDuration));

        currentSequence.Append(levelNameCG.DOFade(1f, itemFadeDuration));
        currentSequence.Join(levelNameRect.DOAnchorPos(levelNameStartPos, itemFadeDuration));

        currentSequence.AppendInterval(staggerDelay);

        currentSequence.Append(timeCG.DOFade(1f, itemFadeDuration));
        currentSequence.Join(timeRect.DOAnchorPos(timeStartPos, itemFadeDuration));

        currentSequence.AppendInterval(staggerDelay);

        currentSequence.Append(deathsCG.DOFade(1f, itemFadeDuration));
        currentSequence.Join(deathsRect.DOAnchorPos(deathsStartPos, itemFadeDuration));

        currentSequence.AppendInterval(staggerDelay);

        currentSequence.Append(collectiblesCG.DOFade(1f, itemFadeDuration));
        currentSequence.Join(collectiblesRect.DOAnchorPos(collectiblesStartPos, itemFadeDuration));
    }

    public void HidePreview()
    {
        currentSequence?.Kill();
        panelCanvasGroup.DOKill();

        if (panelRoot != null)
        {
            panelCanvasGroup.DOFade(0f, panelFadeDuration).OnComplete(() =>
            {
                ResetItemsInstant();
                panelRoot.SetActive(false);
            });
        }
    }

    private void ResetItemsInstant()
    {
        levelNameCG.alpha = 0f;
        timeCG.alpha = 0f;
        deathsCG.alpha = 0f;
        collectiblesCG.alpha = 0f;

        levelNameRect.anchoredPosition = levelNameStartPos + Vector2.up * itemMoveDistance;
        timeRect.anchoredPosition = timeStartPos + Vector2.up * itemMoveDistance;
        deathsRect.anchoredPosition = deathsStartPos + Vector2.up * itemMoveDistance;
        collectiblesRect.anchoredPosition = collectiblesStartPos + Vector2.up * itemMoveDistance;
    }

    private int GetMaxCollectibles(int levelIndex)
    {
        int arrayIndex = levelIndex - 1;

        if (maxCollectiblesPerLevel != null && arrayIndex >= 0 && arrayIndex < maxCollectiblesPerLevel.Length)
            return maxCollectiblesPerLevel[arrayIndex];

        return 0;
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int thousandths = Mathf.FloorToInt(time * 1000f % 1000f);

        return $"{minutes:00}:{seconds:00}.{thousandths:000}";
    }

    private void OnDestroy()
    {
        currentSequence?.Kill();
        panelCanvasGroup.DOKill();
        levelNameCG.DOKill();
        timeCG.DOKill();
        deathsCG.DOKill();
        collectiblesCG.DOKill();
    }
}