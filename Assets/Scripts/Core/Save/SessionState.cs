public static class SessionState
{
    public static string freshLoadSceneName = "";

    public static void MarkFreshLoad(string sceneName)
    {
        freshLoadSceneName = sceneName;
    }

    public static bool IsFreshLoad(string sceneName)
    {
        return freshLoadSceneName == sceneName;
    }

    public static void ClearFreshLoad(string sceneName)
    {
        if (freshLoadSceneName == sceneName)
            freshLoadSceneName = "";
    }
}