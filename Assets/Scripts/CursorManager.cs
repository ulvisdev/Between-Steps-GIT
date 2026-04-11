using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [SerializeField] private InputActionReference _pointerPositionAction;
    [SerializeField] private CanvasGroup _cursorCanvasGroup;
    [SerializeField] private float _fadeDuration = 0.15f;

    private RectTransform _cursorTransform;
    private Canvas _parentCanvas;
    private RectTransform _canvasRectTransform;
    private Camera _canvasCamera;

    private bool _menuMode;
    private Tween _fadeTween;

    private void Awake()
    {
        Instance = this;

        _cursorTransform = GetComponent<RectTransform>();
        _parentCanvas = GetComponentInParent<Canvas>();

        if (_parentCanvas != null)
        {
            _canvasRectTransform = _parentCanvas.GetComponent<RectTransform>();
            _canvasCamera = _parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : _parentCanvas.worldCamera;
        }

    }

    private void OnEnable()
    {
        Cursor.visible = false;
        _pointerPositionAction.action.performed += OnPointerPositionChanged;
        _pointerPositionAction.action.Enable();
    }

    private void OnDisable()
    {
        Cursor.visible = true;
        _pointerPositionAction.action.performed -= OnPointerPositionChanged;
        _fadeTween?.Kill();
    }

    private void OnDestroy()
    {
        _fadeTween?.Kill();
    }

    private void Update()
    {
        if (!_menuMode)
        {
            if (_cursorCanvasGroup.alpha > 0f)
                HideCursor();

            return;
        }

        if (Mouse.current != null && Mouse.current.delta.ReadValue().sqrMagnitude > 0.01f)
        {
            ShowCursor();
            SnapToCurrentMousePosition();
        }

        if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame))
        {
            HideCursor();
        }
    }

    private void OnPointerPositionChanged(InputAction.CallbackContext ctx)
    {
        if (_cursorTransform == null || _canvasRectTransform == null) return;

        var mousePosition = ctx.ReadValue<Vector2>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRectTransform, mousePosition, _canvasCamera, out var localPoint))
        {
            _cursorTransform.anchoredPosition = localPoint;
        }
    }

    private void SnapToCurrentMousePosition()
    {
        if (Mouse.current == null || _cursorTransform == null || _canvasRectTransform == null) return;

        var mousePosition = Mouse.current.position.ReadValue();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRectTransform, mousePosition, _canvasCamera, out var localPoint))
        {
            _cursorTransform.anchoredPosition = localPoint;
        }
    }

    public void SetMenuMode(bool active)
    {
        _menuMode = active;

        if (!_menuMode)
            HideCursor();
    }

    public void ShowCursor()
    {
        FadeCursor(1f);
    }

    public void HideCursor()
    {
        FadeCursor(0f);
    }

    private void FadeCursor(float targetAlpha)
    {
        if (_cursorCanvasGroup == null) return;

        _fadeTween?.Kill();
        _fadeTween = _cursorCanvasGroup.DOFade(targetAlpha, _fadeDuration).SetUpdate(true);
    }
}