using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    private bool finished = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (finished) return;
        if (!collision.CompareTag("Player")) return;

        finished = true;

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