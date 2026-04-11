using UnityEngine;

public class DontDestroyPls : MonoBehaviour
{
    public static DontDestroyPls Instance { get; private set; }
        void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
