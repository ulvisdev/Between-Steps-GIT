using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSessionSaver : MonoBehaviour
{
    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString(SaveSystem.GetLastSceneKey(), sceneName);
        PlayerPrefs.Save();
    }
}