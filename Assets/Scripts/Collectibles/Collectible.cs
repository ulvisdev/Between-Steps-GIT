// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class Collectible : MonoBehaviour
// {
//     [SerializeField] private string collectibleID;
//     [SerializeField] private SpriteRenderer spriteRenderer;
//     [SerializeField] private Color normalColor = Color.white;
//     [SerializeField] private Color collectedBeforeColor = Color.gray;

//     private Animator anim;
//     private bool isCollected;
//     private string levelName;

//     void Start()
//     {
//         anim = GetComponent<Animator>();
//         levelName = SceneManager.GetActiveScene().name;

//         string permanentKey = SaveSystem.GetCollectibleKey(levelName, collectibleID);
//         string runKey = SaveSystem.GetRunCollectibleKey(levelName, collectibleID);

//         bool wasCollectedBefore = PlayerPrefs.GetInt(permanentKey, 0) == 1;
//         bool wasCollectedThisRun = PlayerPrefs.GetInt(runKey, 0) == 1;

//         if (wasCollectedThisRun)
//         {
//             isCollected = true;
//             gameObject.SetActive(false);
//             return;
//         }

//         if (spriteRenderer != null)
//         {
//             spriteRenderer.color = wasCollectedBefore ? collectedBeforeColor : normalColor;
//         }
//     }

//     private void Update()
//     {
//         if (anim != null)
//         {
//             anim.SetBool("isCollected", isCollected);
//         }
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (!collision.CompareTag("Player")) return;
//         if (isCollected) return;

//         isCollected = true;

//         string permanentKey = SaveSystem.GetCollectibleKey(levelName, collectibleID);
//         string runKey = SaveSystem.GetRunCollectibleKey(levelName, collectibleID);

//         PlayerPrefs.SetInt(permanentKey, 1);
//         PlayerPrefs.SetInt(runKey, 1);
//         PlayerPrefs.Save();

//         CollectibleManager.Instance.AddCollectible();

//         if (PlayerLightController.Instance != null)
//         {
//             PlayerLightController.Instance.AddCollectibleLight();
//         }

//         if (AudioManager.Instance != null && AudioManager.Instance.collectibleSFX != null)
//         {
//             AudioManager.Instance.PlaySFX(AudioManager.Instance.collectibleSFX);
//         }

//         GetComponent<Collider2D>().enabled = false;

//         Destroy(gameObject, 0.5f);
//     }
// }

using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectible : MonoBehaviour
{
    [SerializeField] private string collectibleID;
    [SerializeField] private ParticleSystem particles;

    [SerializeField] private Color normalColor = Color.yellow;
    [SerializeField] private Color collectedBeforeColor = Color.gray;

    private Animator anim;
    private bool isCollected;
    private string levelName;

    void Start()
    {
        anim = GetComponent<Animator>();
        levelName = SceneManager.GetActiveScene().name;

        string permanentKey = SaveSystem.GetCollectibleKey(levelName, collectibleID);
        string runKey = SaveSystem.GetRunCollectibleKey(levelName, collectibleID);

        bool wasCollectedBefore = PlayerPrefs.GetInt(permanentKey, 0) == 1;
        bool wasCollectedThisRun = PlayerPrefs.GetInt(runKey, 0) == 1;

        if (wasCollectedThisRun)
        {
            isCollected = true;
            gameObject.SetActive(false);
            return;
        }

        // Set particle color
        if (particles != null)
        {
            var main = particles.main;
            main.startColor = wasCollectedBefore ? collectedBeforeColor : normalColor;
        }
    }

    private void Update()
    {
        if (anim != null)
        {
            anim.SetBool("isCollected", isCollected);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (isCollected) return;

        isCollected = true;

        string permanentKey = SaveSystem.GetCollectibleKey(levelName, collectibleID);
        string runKey = SaveSystem.GetRunCollectibleKey(levelName, collectibleID);

        PlayerPrefs.SetInt(permanentKey, 1);
        PlayerPrefs.SetInt(runKey, 1);
        PlayerPrefs.Save();

        CollectibleManager.Instance.AddCollectible();

        if (PlayerLightController.Instance != null)
        {
            PlayerLightController.Instance.AddCollectibleLight();
        }

        if (AudioManager.Instance != null && AudioManager.Instance.collectibleSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.collectibleSFX);
        }

        GetComponent<Collider2D>().enabled = false;

        // Stop particles before destroying the object
        if (particles != null)
            particles.Stop();

        Destroy(gameObject);
    }
}