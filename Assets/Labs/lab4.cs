using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// UNITY ADVANCED – UI, ANIMATIONS & PHYSICS
/// Attach this script to ONE GameObject
/// Uncomment ONE section at a time

public class UnityAdvancedWorksheet : MonoBehaviour
{
    void Start()
    {
        // Section1_UI();
        // Section2_Animator();
        // Section3_Raycast();
        // Section4_PhysicsMaterials();
        // Section5_SceneManagement();
        // Section6_Events();
    }


    // ===== SECTION 1: UI =====
    void Section1_UI()
    {
        Text uiText = GameObject.Find("UIText")?.GetComponent<Text>();

        if (uiText != null)
        {
            uiText.text = "Hello, Unity!";
            Debug.Log("UI Text updated");
        }

        // TASKS:
        // 1. Create a Canvas and a Text object named "UIText"
        // 2. Press Play
        // 3. Text changes to "Hello, Unity!" and prints message in console
    }


    // ===== SECTION 2: ANIMATOR =====
    void Section2_Animator()
    {
        Animator anim = GetComponent<Animator>();

        if (anim != null)
        {
            anim.SetTrigger("Jump");
            Debug.Log("Jump animation triggered");
        }

        // TASKS:
        // 1. Add Animator component and Animator Controller
        // 2. Create a trigger parameter "Jump" in the controller
        // 3. Press Play
        // 4. Jump animation plays once
    }


    // ===== SECTION 3: RAYCAST =====
    void Section3_Raycast()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f))
        {
            Debug.Log("Hit: " + hit.collider.name);
        }

        // TASKS:
        // 1. Place another GameObject in front
        // 2. Press Play
        // 3. Console prints the object hit
    }


    // ===== SECTION 4: PHYSICS MATERIALS =====
    void Section4_PhysicsMaterials()
    {
        Collider col = GetComponent<Collider>();

        if (col != null)
        {
            PhysicsMaterial mat = new PhysicsMaterial();
            mat.bounciness = 0.8f;
            mat.dynamicFriction = 0.2f;
            col.material = mat;

            Debug.Log("Physics material applied");
        }

        // TASKS:
        // 1. Add Collider (Box, Sphere, etc.)
        // 2. Press Play
        // 3. Object bounces more due to material
    }


    // ===== SECTION 5: SCENE MANAGEMENT =====
    void Section5_SceneManagement()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log("Scene reloaded");
        }

        // TASKS:
        // 1. Press Play
        // 2. Press R key
        // 3. Current scene reloads and console prints message
    }


    // ===== SECTION 6: EVENTS =====
    void Section6_Events()
    {
        Button btn = GameObject.Find("UIButton")?.GetComponent<Button>();

        if (btn != null)
        {
            btn.onClick.AddListener(() =>
            {
                Debug.Log("Button clicked!");
            });
        }

        // TASKS:
        // 1. Create a UI Button named "UIButton"
        // 2. Press Play and click the button
        // 3. Console prints "Button clicked!"
    }
}