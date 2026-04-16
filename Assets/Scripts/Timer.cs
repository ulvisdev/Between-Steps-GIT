// using UnityEngine;
// using TMPro;

// public class Timer : MonoBehaviour
// {
//     [SerializeField] private TextMeshProUGUI timerText;
//     [SerializeField] private float elapsedTime;
//     [SerializeField] private bool isActive = true;
//     private bool timerAllowed;

//     private void Start()
//     {
//         timerAllowed = GameSettings.SpeedrunnerModeEnabled;

//         if (timerText != null)
//             timerText.gameObject.SetActive(timerAllowed);

//         if (!timerAllowed)
//             SetTimerActive(false);

//         UpdateTimerDisplay();
//     }

//     private void Update()
//     {
//         if (!isActive) return;

//         elapsedTime += Time.deltaTime;
//         UpdateTimerDisplay();
//     }

//     private void UpdateTimerDisplay()
//     {
//         int minutes = Mathf.FloorToInt(elapsedTime / 60);
//         int seconds = Mathf.FloorToInt(elapsedTime % 60);
//         int milliseconds = Mathf.FloorToInt(elapsedTime * 1000 % 1000);

//         timerText.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
//     }

//     public void SetTimerActive(bool active)
//     {
//         isActive = active;
//         Debug.Log(active ? "Timer started!" : "Timer paused!");
//     }

//     public void ResetTimer()
//     {
//         elapsedTime = 0f;
//         UpdateTimerDisplay();
//     }

//     public float GetElapsedTime()
//     {
//         return elapsedTime;
//     }
// }

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject timerUI;

    private float elapsedTime;
    private bool isTimerActive;
    private string levelName;

    private string CurrentTimerKey => SaveSystem.GetRunTimerKey(levelName);

    private void Awake()
    {
        levelName = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        bool speedrunMode = RunState.CurrentRunSpeedrunnerMode;

        if (timerUI != null)
            timerUI.SetActive(speedrunMode);

        if (!speedrunMode)
        {
            isTimerActive = false;
            return;
        }

        LoadSavedTime();
        UpdateTimerText();
        isTimerActive = false;
    }

    private void Update()
    {
        if (!RunState.CurrentRunSpeedrunnerMode)
            return;

        if (!isTimerActive)
            return;

        elapsedTime += Time.deltaTime;
        UpdateTimerText();
    }

    public void SetTimerActive(bool active)
    {
        if (!RunState.CurrentRunSpeedrunnerMode)
            return;

        isTimerActive = active;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        isTimerActive = false;
        UpdateTimerText();
    }

    public void SaveTime()
    {
        if (!RunState.CurrentRunSpeedrunnerMode)
            return;

        if (elapsedTime <= 0f)
            return;

        PlayerPrefs.SetFloat(CurrentTimerKey, elapsedTime);
        PlayerPrefs.Save();
    }

    public void LoadSavedTime()
    {
        elapsedTime = PlayerPrefs.GetFloat(CurrentTimerKey, 0f);
    }

    public void ClearSavedTime()
    {
        PlayerPrefs.DeleteKey(CurrentTimerKey);
        PlayerPrefs.Save();
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        int milliseconds = Mathf.FloorToInt(elapsedTime * 1000f % 1000f);

        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }

    public float GetCurrentTime()
    {
        return elapsedTime;
    }

    private void OnDisable()
    {
        SaveTime();
    }

    private void OnApplicationQuit()
    {
        SaveTime();
    }
}