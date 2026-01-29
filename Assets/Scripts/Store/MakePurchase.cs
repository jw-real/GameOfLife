using UnityEngine;
using System.IO;
using TMPro;
using System;

public class MakePurchase : MonoBehaviour
{
    private PlayerProfileData profile;

    public static event System.Action LibraryPurchased;

    public static bool LibraryUnlocked { get; private set; }

    public static event Action PricesChanged;


    [SerializeField] int baseRowCost = 200;
    [SerializeField] int baseColCost = 200;
    [SerializeField] int baseCellCost = 150;
    [SerializeField] private TextMeshProUGUI coinsText;

    void Awake()
    {
        profile = LoadProgressionOrDefaults();
        //LibraryUnlocked = false;
    }
    void OnEnable()
    {
        RefreshUI();
        DeleteRunResult();
    }

    private void RefreshUI()
    {
        if (coinsText != null)
            coinsText.text = profile.coins.ToString();

        PricesChanged?.Invoke();
    }


    public void PurchaseRow()
    {
        int cost = GetRowCost();
        if (profile.coins < cost) return;

        profile.coins -= cost;
        profile.rows++;

        SaveProfile();
        RefreshUI();
    }

    public void PurchaseColumn()
    {
        int cost = GetColumnCost();
        if (profile.coins < cost) return;

        profile.coins -= cost;
        profile.cols++;

        SaveProfile();
        RefreshUI();
    }

    public void PurchaseCell()
    {
        int cost = GetCellCost();
        if (profile.coins < cost) return;

        profile.coins -= cost;
        profile.maxSelectable++;

        SaveProfile();
        RefreshUI();
    }

    public bool TryPurchaseLibrary()
    {
        Debug.Log($"[LibraryPurchase] Attempt. Coins={profile.coins}");

        if (LibraryUnlocked) 
        {
            Debug.Log("[LibraryPurchase] Already unlocked.");   
            return false;
        }

        int cost = 2000;
        if (profile.coins < cost)
        {
            Debug.Log("[LibraryPurchase] Not enough coins.");
            return false;
        }

        profile.coins -= cost;
        LibraryUnlocked = true;

        Debug.Log("[LibraryPurchase] SUCCESS. LibraryUnlocked=true");

        SaveProfile();
        RefreshUI();

        LibraryPurchased?.Invoke();        
        return true;
    }

    public int GetRowCost()
    {
        int purchased = profile.rows;
        return Mathf.RoundToInt(baseRowCost * (1f + purchased * 0.35f));
    }

    public int GetColumnCost()
    {
        int purchased = profile.cols;
        return Mathf.RoundToInt(baseColCost * (1f + purchased * 0.35f));
    }

    public int GetCellCost()
    {
        int purchased = profile.maxSelectable;
        return Mathf.RoundToInt(baseCellCost * (1f + purchased * 0.25f));
    }

    private void SaveProfile()
    {
        string path = Path.Combine(Application.persistentDataPath, "player_profile.json");
        string json = JsonUtility.ToJson(profile, true);
        File.WriteAllText(path, json);
    }

    private PlayerProfileData CreateDefaultProfile()
    {
        return new PlayerProfileData
        {
            rows = 5,
            cols = 5,
            maxSelectable = 5,
            coins = 0
        };
    }

    private PlayerProfileData LoadProgressionOrDefaults()
    {
        string path = Path.Combine(Application.persistentDataPath, "player_profile.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<PlayerProfileData>(json);
        }

        else return CreateDefaultProfile();
    }

    private void DeleteRunResult()
    {
        string path = Path.Combine(Application.persistentDataPath, "run_result.json");

        if (File.Exists(path))
            File.Delete(path);
    }
}