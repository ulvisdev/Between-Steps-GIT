using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    public static string GetCurrentLevelKey()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static string GetCollectibleKey(string levelName, string collectibleID)
    {
        return $"SAVE_COLLECTIBLE_{levelName}_{collectibleID}";
    }

    public static string GetRunCollectibleKey(string levelName, string collectibleID)
    {
        return $"SAVE_RUN_COLLECTIBLE_{levelName}_{collectibleID}";
    }

    public static string GetCollectibleCountKey(string levelName)
    {
        return $"SAVE_COLLECTIBLE_COUNT_{levelName}";
    }

    public static string GetRunCollectibleCountKey(string levelName)
    {
        return $"SAVE_RUN_COLLECTIBLE_COUNT_{levelName}";
    }

    public static string GetLastSceneKey()
    {
        return "SAVE_LAST_SCENE";
    }

    public static string GetLastCheckpointKey(string sceneName)
    {
        return $"SAVE_LAST_CHECKPOINT_{sceneName}";
    }

    public static string GetRunTimerKey(string levelName)
    {
        return $"RUN_TIMER_{levelName}";
    }

    public static string GetRunDeathCountKey(string levelName)
    {
        return $"RUN_DEATHS_{levelName}";
    }

    public static void ResetGameProgress()
    {
        string[] knownLevels = { "TutorialLevel", "Level1", "Level2" };
        string[] collectibleIDs = { "C1", "C2", "C3" };

        foreach (string level in knownLevels)
        {
            PlayerPrefs.DeleteKey(GetRunTimerKey(level));
            PlayerPrefs.DeleteKey(GetRunDeathCountKey(level));

            PlayerPrefs.DeleteKey(GetCollectibleCountKey(level));
            PlayerPrefs.DeleteKey(GetRunCollectibleCountKey(level));
            PlayerPrefs.DeleteKey(GetLastCheckpointKey(level));

            foreach (string collectibleID in collectibleIDs)
            {
                PlayerPrefs.DeleteKey(GetCollectibleKey(level, collectibleID));
                PlayerPrefs.DeleteKey(GetRunCollectibleKey(level, collectibleID));
            }
        }

        PlayerPrefs.DeleteKey("HighestLevel");
        PlayerPrefs.DeleteKey(GetLastSceneKey());

        PlayerPrefs.Save();
    }

    public static void ResetLevelRunProgress(string levelName)
    {
        string[] collectibleIDs = { "C1", "C2", "C3" };

        PlayerPrefs.DeleteKey(GetRunTimerKey(levelName));
        PlayerPrefs.DeleteKey(GetRunDeathCountKey(levelName));
        PlayerPrefs.DeleteKey(GetRunCollectibleCountKey(levelName));
        PlayerPrefs.DeleteKey(GetLastCheckpointKey(levelName));

        foreach (string collectibleID in collectibleIDs)
        {
            PlayerPrefs.DeleteKey(GetRunCollectibleKey(levelName, collectibleID));
        }

        PlayerPrefs.Save();
    }
}