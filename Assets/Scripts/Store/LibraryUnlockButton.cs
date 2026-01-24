using UnityEngine;

public class LibraryUnlockButton : MonoBehaviour
{
    [SerializeField] private GameObject libraryAccessIcon;
    [SerializeField] private MakePurchase makePurchase;

    private void OnEnable()
    {
        MakePurchase.LibraryPurchased += OnLibraryPurchased;
        SyncInitialState();
    }

    private void OnDisable()
    {
        MakePurchase.LibraryPurchased -= OnLibraryPurchased;
    }

    private void OnLibraryPurchased()
    {
        libraryAccessIcon.SetActive(true);
        gameObject.SetActive(false); // hide purchase button
    }

    private void SyncInitialState()
    {
        if (MakePurchase.LibraryUnlocked)
        {
            // Already unlocked earlier in this session
            libraryAccessIcon.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            // Not yet unlocked
            libraryAccessIcon.SetActive(false);
            gameObject.SetActive(true);
        }
    }

    // Called by Button OnClick
    public void OnPurchaseClicked()
    {
        makePurchase.TryPurchaseLibrary();
    }
}