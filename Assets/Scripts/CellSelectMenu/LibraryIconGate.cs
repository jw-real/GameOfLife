using UnityEngine;

public class LibraryIconGate : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("[LibraryIconGate] Awake fired");
        gameObject.SetActive(MakePurchase.LibraryUnlocked);
    }
    private void OnEnable()
    {
        Debug.Log($"[LibraryIconGate] OnEnable. LibraryUnlocked={MakePurchase.LibraryUnlocked}");
        SyncInitialState();
        //gameObject.SetActive(SessionState.Instance.LibraryUnlocked);
    }

    private void SyncInitialState()
    {
        //gameObject.SetActive(MakePurchase.LibraryUnlocked);
        //gameObject.SetActive(SessionState.Instance.LibraryUnlocked);
        Debug.Log($"[LibraryIconGate] SyncInitialState → {MakePurchase.LibraryUnlocked}");
    }

    void Update()
    {
        Debug.Log($"LibraryIcon activeSelf={gameObject.activeSelf}, activeInHierarchy={gameObject.activeInHierarchy}");
        enabled = false; // prevent spam
    }
}