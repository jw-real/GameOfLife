using UnityEngine;

public class SessionState : MonoBehaviour
{
    public static SessionState Instance { get; private set; }

    public bool LibraryUnlocked { get; private set; } = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UnlockLibrary()
    {
        LibraryUnlocked = true;
    }
}