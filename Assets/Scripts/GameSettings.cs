using UnityEngine;

public static class GameSettings
{
    private const string SpeedrunnerModeKey = "SpeedrunnerMode";

    public static bool SpeedrunnerModeEnabled
    {
        get => PlayerPrefs.GetInt(SpeedrunnerModeKey, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(SpeedrunnerModeKey, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}