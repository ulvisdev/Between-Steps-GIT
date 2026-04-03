using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }

    public int collectibleCount = 0;
    public int totalCollectibles = 3;
    public TMP_Text collectibleText;
    public UICollectiblePunch collectiblePunch;

    private string levelName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        levelName = SceneManager.GetActiveScene().name;

        collectibleCount = PlayerPrefs.GetInt(SaveSystem.GetRunCollectibleCountKey(levelName), 0);

        UpdateCollectibleUI();
    }

    // private void Start()
    // {
    //     SessionState.ClearFreshLoad(levelName);
    // }

    public void AddCollectible()
    {
        collectibleCount++;
        PlayerPrefs.SetInt(SaveSystem.GetRunCollectibleCountKey(levelName), collectibleCount);
        PlayerPrefs.Save();

        UpdateCollectibleUI();

        if (collectiblePunch != null)
        {
            collectiblePunch.Punch();
        }
    }

    private void UpdateCollectibleUI()
    {
        if (collectibleText != null)
        {
            collectibleText.text = $"{collectibleCount}/{totalCollectibles}";
        }
    }
}