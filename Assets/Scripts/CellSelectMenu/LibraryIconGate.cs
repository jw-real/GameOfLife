using UnityEngine;

public class LibraryIconGate : MonoBehaviour
{
    private void OnEnable()
    {
    //    SyncInitialState();
        gameObject.SetActive(SessionState.Instance.LibraryUnlocked);
    }

    //private void SyncInitialState()
    //{
    //    gameObject.SetActive(MakePurchase.LibraryUnlocked);
    //}
}