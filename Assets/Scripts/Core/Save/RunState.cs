using UnityEngine;

public static class RunState
{
    private const string CurrentRunSpeedrunnerKey = "CURRENT_RUN_SPEEDRUNNER_MODE";

    public static bool CurrentRunSpeedrunnerMode
    {
        get => PlayerPrefs.GetInt(CurrentRunSpeedrunnerKey, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(CurrentRunSpeedrunnerKey, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static void ApplySettingsToNewRun()
    {
        CurrentRunSpeedrunnerMode = GameSettings.SpeedrunnerModeEnabled;
    }
}