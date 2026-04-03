using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    [SerializeField] private Button button;
    [SerializeField] private GameObject lockIcon;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
    }

    public void Setup(int highestUnlockedLevel)
    {
        bool unlocked = levelIndex <= highestUnlockedLevel;

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
}