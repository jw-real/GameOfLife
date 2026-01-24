using UnityEngine;
using System.IO;
using TMPro;

public class MakePurchase : MonoBehaviour
{
    private PlayerProfileData profile;

    [SerializeField] int baseRowCost = 200;
    [SerializeField] int baseColCost = 200;
    [SerializeField] int baseCellCost = 150;
    [SerializeField] private TextMeshProUGUI coinsText;

    void Start()
    {
        profile = LoadProgressionOrDefaults();
        RefreshUI();
        DeleteRunResult();
    }

    private void RefreshUI()
    {
        if (coinsText != null)
            coinsText.text = profile.coins.ToString();
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
        int cost = 2000;
        if (profile.coins < cost) return false;

        profile.coins -= cost;
        
        SaveProfile();
        RefreshUI();

        return true;
    }

    int GetRowCost()
    {
        int purchased = profile.rows;
        return Mathf.RoundToInt(baseRowCost * (1f + purchased * 0.35f));
    }

    int GetColumnCost()
    {
        int purchased = profile.cols;
        return Mathf.RoundToInt(baseColCost * (1f + purchased * 0.35f));
    }

    int GetCellCost()
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
