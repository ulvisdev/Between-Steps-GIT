using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private Transform checkpointTransform;
    [SerializeField] private string checkpointID; // <-- NEW

    //private bool activated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (activated) return;
        if (!collision.CompareTag("Player")) return;

        PlayerController player = collision.GetComponent<PlayerController>();
        if (player == null) return;

        Transform pointToUse = checkpointTransform != null ? checkpointTransform : transform;
        player.SetCheckpoint(pointToUse);

        // SAVE CHECKPOINT
        string sceneName = SceneManager.GetActiveScene().name;

        PlayerPrefs.SetString(SaveSystem.GetLastSceneKey(), sceneName);
        PlayerPrefs.SetString(SaveSystem.GetLastCheckpointKey(sceneName), checkpointID);
        PlayerPrefs.Save();

        Debug.Log($"Checkpoint saved: {sceneName} - {checkpointID}");

        Timer timer = FindFirstObjectByType<Timer>();
        if (timer != null)
            timer.SaveTime();

        //activated = true;
    }
}