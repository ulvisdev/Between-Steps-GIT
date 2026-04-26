using UnityEngine;
using UnityEngine.InputSystem;

public class InputLoader : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    private void Awake()
    {
        
        string rebinds = PlayerPrefs.GetString("InputBindings", "");

        if (!string.IsNullOrEmpty(rebinds))
        {
            inputActions.LoadBindingOverridesFromJson(rebinds);
        }
    }
}