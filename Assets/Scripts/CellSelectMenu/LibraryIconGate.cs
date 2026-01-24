using UnityEngine;

public class LibraryIconGate : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.SetActive(SessionState.Instance.LibraryUnlocked);
    }
}