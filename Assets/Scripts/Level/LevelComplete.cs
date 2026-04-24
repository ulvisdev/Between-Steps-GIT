using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public void CompleteLevel()
    {
        string levelName = SceneManager.GetActiveScene().name;

        Timer timer = FindFirstObjectByType<Timer>();
        CollectibleManager collectibleManager = FindFirstObjectByType<CollectibleManager>();

        if (timer != null && RunState.CurrentRunSpeedrunnerMode)
        {
            float finalTime = timer.GetCurrentTime();

            if (finalTime > 0f)
                LevelStats.SaveBestTime(levelName, finalTime);
        }

        if (collectibleManager != null)
            LevelStats.SaveBestCollectibles(levelName, collectibleManager.GetCollectedCount());

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        int highestUnlocked = PlayerPrefs.GetInt("HighestLevel", 1);

        if (nextSceneIndex > highestUnlocked)
        {
            PlayerPrefs.SetInt("HighestLevel", nextSceneIndex);
            PlayerPrefs.Save();
        }

        Debug.Log("Saved HighestLevel = " + PlayerPrefs.GetInt("HighestLevel"));

        if (SceneLoader.Instance != null)
            SceneLoader.Instance.NextLevel();
    }
}