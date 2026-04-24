using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private int levelIndex;
    [SerializeField] private Button button;
    [SerializeField] private GameObject lockIcon;

    private bool unlocked;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
    }

    public void Setup(int highestUnlockedLevel)
    {
        unlocked = levelIndex <= highestUnlockedLevel;

        button.interactable = unlocked;

        if (lockIcon != null)
            lockIcon.SetActive(!unlocked);
    }

    public void LoadLevel()
    {
        int highestUnlockedLevel = PlayerPrefs.GetInt("HighestLevel", 1);

        if (levelIndex > highestUnlockedLevel) return;

        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadScene("Level" + levelIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowPreview();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (EventSystem.current == null || EventSystem.current.currentSelectedGameObject != gameObject)
            HidePreview();
    }

    public void OnSelect(BaseEventData eventData)
    {
        ShowPreview();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        HidePreview();
    }

private void ShowPreview()
{
    // Debug.Log("ShowPreview called for levelIndex = " + levelIndex + " on object " + gameObject.name);

    if (LevelPreviewUI.Instance != null)
        LevelPreviewUI.Instance.ShowPreview(levelIndex, unlocked);
}

    private void HidePreview()
    {
        if (LevelPreviewUI.Instance != null)
            LevelPreviewUI.Instance.HidePreview();
    }
}