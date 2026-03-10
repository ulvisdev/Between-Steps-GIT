using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float backupRespawnPointX = -4f;
    [SerializeField] private float backupRespawnPointY = -0.4f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Rigidbody2D rb = collision.attachedRigidbody; // takes player rigidbody
        if (rb == null) rb = collision.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Vector2 pos = respawnPoint != null
            ? (Vector2)respawnPoint.position
            : new Vector2(backupRespawnPointX, backupRespawnPointY);

        // move player
        rb.transform.position = pos;
    }
}