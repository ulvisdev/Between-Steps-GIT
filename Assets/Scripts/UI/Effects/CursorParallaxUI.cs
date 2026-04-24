using UnityEngine;
using UnityEngine.InputSystem;

public class CursorParallaxUI : MonoBehaviour
{
    [SerializeField] private RectTransform target;
    [SerializeField] private float strength = 25f;
    [SerializeField] private float smoothSpeed = 6f;

    private Vector2 startPos;

    private void Start()
    {
        if (target == null)
            target = GetComponent<RectTransform>();

        startPos = target.anchoredPosition;
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 offset = (mousePos - screenCenter) / screenCenter;

        Vector2 targetPos = startPos + offset * strength;

        target.anchoredPosition = Vector2.Lerp(
            target.anchoredPosition,
            targetPos,
            Time.unscaledDeltaTime * smoothSpeed
        );
    }
}