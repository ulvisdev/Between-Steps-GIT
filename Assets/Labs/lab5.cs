using UnityEngine;
using UnityEngine.UI;

/// UNITY 2D – LAB 5
/// Attach this script to ONE GameObject
/// Uncomment ONE section at a time

public class Unity2DLab5 : MonoBehaviour
{
    void Start()
    {
        // Section1_SpriteColor();
        // Section2_Animator2D();
        // Section3_SimpleParticles2D();
        // Section4_UIUpdate2D();
        // Section5_FollowMouse();
    }

    // ===== SECTION 1: SPRITE COLOR =====
    void Section1_SpriteColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.color = Color.yellow;
            Debug.Log("Sprite color changed to yellow");
        }

        // TASKS:
        // 1. Add a SpriteRenderer to this GameObject
        // 2. Press Play
        // 3. Sprite turns yellow, console prints message
    }

    // ===== SECTION 2: ANIMATOR 2D =====
    void Section2_Animator2D()
    {
        Animator anim = GetComponent<Animator>();

        if (anim != null)
        {
            anim.SetTrigger("Jump");
            Debug.Log("2D Jump animation triggered");
        }

        // TASKS:
        // 1. Add Animator and create a 2D animation controller
        // 2. Create a trigger "Jump" and assign animation
        // 3. Press Play to trigger animation
    }

    // ===== SECTION 3: SIMPLE PARTICLES 2D =====
    void Section3_SimpleParticles2D()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        if (ps != null)
        {
            ps.Play();
            Debug.Log("2D particle system playing");
        }

        // TASKS:
        // 1. Add ParticleSystem component
        // 2. Set Shape to Circle or Cone (2D friendly)
        // 3. Press Play to see particles emit
    }

    // ===== SECTION 4: UI UPDATE 2D =====
    void Section4_UIUpdate2D()
    {
        Text uiText = GameObject.Find("ScoreText")?.GetComponent<Text>();

        if (uiText != null)
        {
            uiText.text = "Score: 100";
            Debug.Log("UI updated with score");
        }

        // TASKS:
        // 1. Create a Canvas and Text object named "ScoreText"
        // 2. Press Play to see score change
    }

    // ===== SECTION 5: FOLLOW MOUSE =====
    void Section5_FollowMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Keep in 2D plane
        transform.position = mousePos;

        // TASKS:
        // 1. Press Play
        // 2. GameObject follows mouse in 2D
    }
}