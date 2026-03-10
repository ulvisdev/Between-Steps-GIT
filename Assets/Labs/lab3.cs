using UnityEngine;

/// UNITY INTERMEDIATE – CORE UNITY CONCEPTS 2
/// Attach this script to ONE GameObject
/// Uncomment ONE section at a time

public class UnityIntermediateWorksheet : MonoBehaviour
{
    void Start()
    {
        // Section1_Prefabs();
        // Section2_Materials();
        // Section3_Input();
        // Section4_Audio();
        // Section5_ScriptedMovement();
        // Section6_Coroutines();
    }


    // ===== SECTION 1: PREFABS =====
    void Section1_Prefabs()
    {
        // TASKS:
        // 1. Create a prefab called "Bullet"
        // 2. Instantiate it at the GameObject's position
        // 3. Console should print "Bullet spawned"
        GameObject bulletPrefab = Resources.Load<GameObject>("Bullet");

        if (bulletPrefab != null)
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Debug.Log("Bullet spawned");
        }
        else
        {
            Debug.LogWarning("Bullet prefab not found in Resources folder");
        }
    }


    // ===== SECTION 2: MATERIALS =====
    void Section2_Materials()
    {
        Renderer rend = GetComponent<Renderer>();

        if (rend != null)
        {
            rend.material.color = Color.green;
            Debug.Log("Material color changed to green");
        }

        // TASKS:
        // 1. Add a Renderer component (e.g., MeshRenderer)
        // 2. Console should print: "Material color changed to green"
    }


    // ===== SECTION 3: INPUT =====
    void Section3_Input()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key pressed");
        }

        // TASKS:
        // 1. Press Play
        // 2. Press Space key
        // 3. Console should print: "Space key pressed"
    }


    // ===== SECTION 4: AUDIO =====
    void Section4_Audio()
    {
        AudioSource audio = GetComponent<AudioSource>();

        if (audio != null && !audio.isPlaying)
        {
            audio.Play();
            Debug.Log("Audio played");
        }

        // TASKS:
        // 1. Add an AudioSource component
        // 2. Assign an audio clip
        // 3. Console should print: "Audio played" when Play is pressed
    }


    // ===== SECTION 5: SCRIPTED MOVEMENT =====
    void Section5_ScriptedMovement()
    {
        float speed = 5f;
        float move = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * move * speed * Time.deltaTime);

        // TASKS:
        // 1. Press Play
        // 2. Use arrow keys or A/D keys
        // 3. GameObject moves horizontally
    }


    // ===== SECTION 6: COROUTINES =====
    void Section6_Coroutines()
    {
        StartCoroutine(ChangeColorRoutine());

        // TASKS:
        // 1. Press Play
        // 2. Object color changes every 2 seconds
    }

    System.Collections.IEnumerator ChangeColorRoutine()
    {
        Renderer rend = GetComponent<Renderer>();

        while (true)
        {
            if (rend != null)
            {
                rend.material.color = new Color(Random.value, Random.value, Random.value);
                Debug.Log("Color changed");
            }
            yield return new WaitForSeconds(2f);
        }
    }
}