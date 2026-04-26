using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuInputHandler : MonoBehaviour
{
    public CanvasGroup menuRoot;
    public Selectable firstSelectable;
    public Selectable backupSelectable;

    void Update()
    {
        HandleAutoSelection();
    }

    void HandleAutoSelection()
    {
        if (EventSystem.current == null)
            return;

        if (menuRoot == null || !menuRoot.gameObject.activeInHierarchy)
            return;

        if (EventSystem.current.currentSelectedGameObject != null)
            return;

        if (!NonPointerInputDetected())
            return;

        if (firstSelectable != null && firstSelectable.IsInteractable())
        {
            EventSystem.current.SetSelectedGameObject(firstSelectable.gameObject);
        }
        else if (backupSelectable != null && backupSelectable.IsInteractable())
        {
            EventSystem.current.SetSelectedGameObject(backupSelectable.gameObject);
        }
    }

    bool NonPointerInputDetected()
    {
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            return true;

        if (Gamepad.current != null)
        {
            if (Gamepad.current.leftStick.ReadValue().sqrMagnitude > 0.1f)
                return true;

            if (Gamepad.current.dpad.ReadValue().sqrMagnitude > 0.1f)
                return true;

            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
                return true;
        }

        return false;
    }

    public void ClearSelection()
    {
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }
}