using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class PurchaseButtonUI : MonoBehaviour
{
    public enum PurchaseType
    {
        Row,
        Column,
        Cell,
        Library
    }

    [Header("Config")]
    [SerializeField] private PurchaseType purchaseType;
    [SerializeField] private MakePurchase makePurchase;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private LocalizedString localizedLabel;

    private void Start()
    {
        localizedLabel.StringChanged += OnStringChanged;
        Refresh();
    }

    private void OnDisable()
    {
        localizedLabel.StringChanged -= OnStringChanged;
    }

    private void OnStringChanged(string value)
    {
        Refresh();
    }

    public void Refresh()
    {
        int cost = GetCost();
        string baseText = localizedLabel.GetLocalizedString();
        label.text = $"{baseText}: <sprite name=\"coin\"> {cost}";
    }

    private int GetCost()
    {
        return purchaseType switch
        {
            PurchaseType.Row    => makePurchase.GetRowCost(),
            PurchaseType.Column => makePurchase.GetColumnCost(),
            PurchaseType.Cell   => makePurchase.GetCellCost(),
            PurchaseType.Library => 2000,
            _ => 0
        };
    }
}