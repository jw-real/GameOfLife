using UnityEngine;
using System.IO;
/*
public class MakePurchase : MonoBehaviour
{
    private ProgressionData profile;

    [SerializeField] int baseRowCost = 200;
    [SerializeField] int baseColCost = 200;
    [SerializeField] int baseCellCost = 150;

    void Start()
    {
        profile = LoadProgressionOrDefaults();
    }

    public void PurchaseRow()
    {
        int cost = GetRowCost();
        if (profile.coins < cost) return;

        profile.coins -= cost;
        profile.rows++;

        SaveProfile();
    }

    public void PurchaseColumn()
    {
        int cost = GetColumnCost();
        if (profile.coins < cost) return;

        profile.coins -= cost;
        profile.cols++;

        SaveProfile();
    }

    public void PurchaseCell()
    {
        int cost = GetCellCost();
        if (profile.coins < cost) return;

        profile.coins -= cost;
        profile.maxSelectable++;

        SaveProfile();
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

    private ProgressionData LoadProgressionOrDefaults()
    {
        string path = Path.Combine(Application.persistentDataPath, "player_profile.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<ProgressionData>(json);
        }

        return new ProgressionData(startRows, startCols, startCells);
    }
}
*/