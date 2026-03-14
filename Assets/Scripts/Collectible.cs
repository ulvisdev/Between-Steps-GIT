using UnityEngine;

public class Collectible : MonoBehaviour
{
    //[SerializeField] private float rotationSpeed = 90f; // degrees per second
    private Animator anim;
    private bool isCollected;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        /*if (!isCollected)
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }*/

        if (anim != null)
        {
            anim.SetBool("isCollected", isCollected);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollected = true;
            GameManager.Instance.AddCollectible();
            if (AudioManager.Instance != null && AudioManager.Instance.collectibleSFX != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.collectibleSFX);
            }

            GetComponent<Collider2D>().enabled = false;

            Destroy(gameObject, 0.5f);
        }
    }
}
