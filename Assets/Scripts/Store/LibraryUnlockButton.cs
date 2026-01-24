using UnityEngine;

public class LibraryUnlockButton : MonoBehaviour
{
    [SerializeField] private MakePurchase makePurchases;
    [SerializeField] private GameObject purchaseButton;
    [SerializeField] private GameObject libraryAccessIcon;

    private bool libraryUnlocked;

    public void OnPurchaseClicked()
    {
        if (libraryUnlocked)
            return;

        if (!makePurchases.TryPurchaseLibrary())
        {
            Debug.Log("Insufficient currency");
            return;
        }

        libraryUnlocked = true;
        UpdateUI();
    }

    private void UpdateUI()
    {
        purchaseButton.SetActive(!libraryUnlocked);
        libraryAccessIcon.SetActive(libraryUnlocked);
    }
}