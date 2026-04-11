using UnityEngine;

public static class LevelStats
{
    public static string GetBestTimeKey(string sceneName) => $"BEST_TIME_{sceneName}";
    public static string GetBestDeathsKey(string sceneName) => $"BEST_DEATHS_{sceneName}";
    public static string GetBestCollectiblesKey(string sceneName) => $"BEST_COLLECTIBLES_{sceneName}";

    public static void SaveBestTime(string sceneName, float newTime)
    {
        string key = GetBestTimeKey(sceneName);

        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetFloat(key, newTime);
            PlayerPrefs.Save();
            return;
        }

        float currentBest = PlayerPrefs.GetFloat(key);

        if (newTime < currentBest)
        {
            PlayerPrefs.SetFloat(key, newTime);
            PlayerPrefs.Save();
        }
    }

    public static float GetBestTime(string sceneName)
    {
        return PlayerPrefs.GetFloat(GetBestTimeKey(sceneName), -1f);
    }

    public static void SaveBestDeaths(string sceneName, int newDeaths)
    {
        string key = GetBestDeathsKey(sceneName);

        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt(key, newDeaths);
            PlayerPrefs.Save();
            return;
        }

        int currentBest = PlayerPrefs.GetInt(key);

        if (newDeaths < currentBest)
        {
            PlayerPrefs.SetInt(key, newDeaths);
            PlayerPrefs.Save();
        }
    }

    public static int GetBestDeaths(string sceneName)
    {
        return PlayerPrefs.GetInt(GetBestDeathsKey(sceneName), -1);
    }

    public static void SaveBestCollectibles(string sceneName, int collected)
    {
        string key = GetBestCollectiblesKey(sceneName);

        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt(key, collected);
            PlayerPrefs.Save();
            return;
        }

        int currentBest = PlayerPrefs.GetInt(key);

        if (collected > currentBest)
        {
            PlayerPrefs.SetInt(key, collected);
            PlayerPrefs.Save();
        }
    }

    public static int GetBestCollectibles(string sceneName)
    {
        return PlayerPrefs.GetInt(GetBestCollectiblesKey(sceneName), 0);
    }

    public static void ResetLevelStats(string sceneName)
    {
        PlayerPrefs.DeleteKey(GetBestTimeKey(sceneName));
        PlayerPrefs.DeleteKey(GetBestDeathsKey(sceneName));
        PlayerPrefs.DeleteKey(GetBestCollectiblesKey(sceneName));
        PlayerPrefs.Save();
    }
}