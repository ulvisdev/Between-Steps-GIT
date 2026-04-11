using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private bool finished = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (finished) return;
        if (!collision.CompareTag("Player")) return;

        finished = true;

        LevelComplete levelComplete = FindFirstObjectByType<LevelComplete>();

        if (levelComplete != null)
            levelComplete.CompleteLevel();
        else
            Debug.LogError("LevelComplete not found in scene.");
    }
}