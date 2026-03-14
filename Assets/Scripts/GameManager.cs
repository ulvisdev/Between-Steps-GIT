using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int collectibleCount = 0;
    public TMP_Text collectibleText;
    public UICollectiblePunch collectiblePunch;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        collectibleText.text = $"{collectibleCount}/3";
    }

    public void AddCollectible()
    {
        collectibleCount++;
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
            collectibleText.text = $"{collectibleCount}/3";
        }
    }
}
