using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointLoader : MonoBehaviour
{
    [System.Serializable]
    public class CheckpointData
    {
        public string checkpointID;
        public Transform point;
    }

    public PlayerController player;
    public CheckpointData[] checkpoints;

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string savedID = PlayerPrefs.GetString(SaveSystem.GetLastCheckpointKey(sceneName), "");

        if (string.IsNullOrEmpty(savedID))
            return;

        foreach (var cp in checkpoints)
        {
            if (cp.checkpointID == savedID)
            {
                player.SetCheckpoint(cp.point);
                player.transform.position = cp.point.position;
                break;
            }
        }
    }
}