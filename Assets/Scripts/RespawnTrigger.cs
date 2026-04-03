using UnityEngine;
using System.Collections;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float backupRespawnPointX = -4f;
    [SerializeField] private float backupRespawnPointY = -0.4f;

    [SerializeField] private float freezeTime = 0.3f;
    [SerializeField] private float transitionTime = 0.2f;

    private bool isRespawning = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isRespawning) return;
        if (!collision.CompareTag("Player")) return;

        Rigidbody2D rb = collision.attachedRigidbody; // takes player rigidbody
        if (rb == null) rb = collision.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        PlayerController player = collision.GetComponent<PlayerController>();
        if (player == null) return;

        StartCoroutine(RespawnSequence(player, rb));
    }

    private IEnumerator RespawnSequence(PlayerController player, Rigidbody2D rb)
    {
        isRespawning = true;

        Vector2 pos;
        if (player.currentCheckpoint != null) pos = player.currentCheckpoint.position;
        else if (respawnPoint != null) pos = respawnPoint.position;
        else pos = new Vector2(backupRespawnPointX, backupRespawnPointY);

        Animator anim = player.GetComponent<Animator>();

        player.Die();
        if (anim != null)
            anim.SetBool("isDead", true);

        yield return new WaitForSeconds(freezeTime);

        if (ScreenFader.Instance != null)
            yield return StartCoroutine(ScreenFader.Instance.FadeOut());
        yield return new WaitForSeconds(transitionTime);

        // move player 
        rb.position = pos;

        rb.linearVelocity = Vector2.zero;

        if (anim != null)
            anim.SetBool("isDead", false);

        yield return new WaitForSeconds(transitionTime);

        player.Revive();

        if (ScreenFader.Instance != null)
            yield return StartCoroutine(ScreenFader.Instance.FadeIn());

        isRespawning = false;

    }

}