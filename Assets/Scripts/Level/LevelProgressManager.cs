using UnityEngine;

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager Instance;

    private const string HighestUnlockedKey = "HighestUnlockedLevel";

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
            return;
        }

        if (!PlayerPrefs.HasKey(HighestUnlockedKey))
        {
            PlayerPrefs.SetInt(HighestUnlockedKey, 1);
            PlayerPrefs.Save();
        }
    }

    public int GetHighestUnlockedLevel()
    {
        return PlayerPrefs.GetInt(HighestUnlockedKey, 1);
    }

    public bool IsLevelUnlocked(int levelIndex)
    {
        return levelIndex <= GetHighestUnlockedLevel();
    }

    public void UnlockNextLevel(int completedLevelIndex)
    {
        int nextLevel = completedLevelIndex + 1;
        int currentHighest = GetHighestUnlockedLevel();

        if (nextLevel > currentHighest)
        {
            PlayerPrefs.SetInt(HighestUnlockedKey, nextLevel);
            PlayerPrefs.Save();
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.SetInt(HighestUnlockedKey, 1);
        PlayerPrefs.Save();
    }
}