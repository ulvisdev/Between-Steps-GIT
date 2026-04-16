using UnityEngine;

public static class LevelStats
{
    public static string GetBestTimeKey(string sceneName) => $"BEST_TIME_{sceneName}";
    public static string GetTotalDeathsKey(string sceneName) => $"TOTAL_DEATHS_{sceneName}";
    public static string GetBestCollectiblesKey(string sceneName) => $"BEST_COLLECTIBLES_{sceneName}";

    public static void SaveBestTime(string sceneName, float newTime)
    {
        if (newTime <= 0f)
            return;

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

    public static void AddDeaths(string sceneName, int deathsToAdd)
    {
        string key = GetTotalDeathsKey(sceneName);
        int currentDeaths = PlayerPrefs.GetInt(key, 0);

        currentDeaths += deathsToAdd;

        PlayerPrefs.SetInt(key, currentDeaths);
        PlayerPrefs.Save();
    }

    public static int GetTotalDeaths(string sceneName)
    {
        return PlayerPrefs.GetInt(GetTotalDeathsKey(sceneName), 0);
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
        PlayerPrefs.DeleteKey(GetTotalDeathsKey(sceneName));
        PlayerPrefs.DeleteKey(GetBestCollectiblesKey(sceneName));
        PlayerPrefs.Save();
    }
}