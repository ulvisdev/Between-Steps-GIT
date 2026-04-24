using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Keybind : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI buttonLbl;

    [Header("Input")]
    [SerializeField] private InputActionReference actionToRebind;

    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    private void Start()
    {
        LoadBindings();
        UpdateLabel();
    }

    public void ChangeKey()
    {
        buttonLbl.text = "Awaiting Input...";

        actionToRebind.action.Disable();

        rebindOperation = actionToRebind.action.PerformInteractiveRebinding().WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
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

    private void UpdateLabel()
    {
        buttonLbl.text = actionToRebind.action.GetBindingDisplayString();
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
}