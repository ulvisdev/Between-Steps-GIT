using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int collectibleCount = 0;
    public TMP_Text collectibleText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        collectibleText.text = $"{collectibleCount}";
    }

    public void AddCollectible()
    {
        collectibleCount++;
        UpdateCollectibleUI();
    }

    private void UpdateCollectibleUI()
    {
        if (collectibleText != null)
        {
            collectibleText.text = $"{collectibleCount}";
        }
    }
}
