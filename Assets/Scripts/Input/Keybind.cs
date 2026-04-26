using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Keybind : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI buttonLbl;

    [Header("Input")]
    [SerializeField] private InputActionReference actionToRebind;
    [SerializeField] private int bindingIndex = 0;

    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    private void Start()
    {
        LoadBindings();
        UpdateLabel();
    }

    public void ChangeKey()
    {
        int index = GetRealBindingIndex();

        buttonLbl.text = "Awaiting";

        actionToRebind.action.Disable();

        rebindOperation = actionToRebind.action
            .PerformInteractiveRebinding(index)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .WithCancelingThrough("<Gamepad>/buttonEast")
            .OnComplete(operation =>
            {
                operation.Dispose();
                actionToRebind.action.Enable();

                SaveBindings();
                UpdateLabel();
            })
            .OnCancel(operation =>
            {
                operation.Dispose();
                actionToRebind.action.Enable();

                UpdateLabel();
            })
            .Start();
    }

    private int GetRealBindingIndex()
    {
        if (actionToRebind.action.bindings[bindingIndex].isComposite)
        {
            return bindingIndex + 1;
        }

        return bindingIndex;
    }

    private void UpdateLabel()
    {
        int index = GetRealBindingIndex();
        buttonLbl.text = actionToRebind.action.GetBindingDisplayString(index);
    }

    private void SaveBindings()
    {
        string rebinds = actionToRebind.action.actionMap.asset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("InputBindings", rebinds);
        PlayerPrefs.Save();
    }

    private void LoadBindings()
    {
        string rebinds = PlayerPrefs.GetString("InputBindings", "");

        if (!string.IsNullOrEmpty(rebinds))
        {
            actionToRebind.action.actionMap.asset.LoadBindingOverridesFromJson(rebinds);
        }
    }

    private void OnDisable()
    {
        rebindOperation?.Dispose();
    }
}