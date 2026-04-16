using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerLightController : MonoBehaviour
{
    public static PlayerLightController Instance;

    [SerializeField] private Light2D playerLight;

    private Tween lightTween;

    [Header("Light Settings")]
    [SerializeField] private float baseRadius = 2f;
    [SerializeField] private float radiusPerCollectible = 0.5f;
    [SerializeField] private float maxRadius = 6f;

    private int collectedThisLevel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResetLightForLevel();
        RebuildLightFromSavedCollectibles();
    }

    public void AddCollectibleLight()
    {
        collectedThisLevel++;
        UpdateLight();
    }

    public void ResetLightForLevel()
    {
        collectedThisLevel = 0;
        UpdateLight();
    }

    public void RebuildLightFromSavedCollectibles()
    {
        string levelName = SceneManager.GetActiveScene().name;
        int savedCount = PlayerPrefs.GetInt($"SAVE_COLLECTIBLE_COUNT_{levelName}", 0);

        collectedThisLevel = savedCount;
        UpdateLight();
    }

    private void UpdateLight()
    {
        if (playerLight == null) return;

        float targetRadius = baseRadius + (collectedThisLevel * radiusPerCollectible);
        targetRadius = Mathf.Clamp(targetRadius, baseRadius, maxRadius);

        lightTween?.Kill();
        lightTween = DOTween.To(() => playerLight.pointLightOuterRadius, x => playerLight.pointLightOuterRadius = x, targetRadius, 0.25f);

        Debug.Log($"Updated light radius to {targetRadius} based on {collectedThisLevel} collectibles.");
    }

    public int GetCollectedCount()
    {
        return collectedThisLevel;
    }
}