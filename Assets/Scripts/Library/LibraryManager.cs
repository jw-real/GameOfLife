using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LibraryManager : MonoBehaviour
{
    private const string LibraryFileName = "pattern_library_truncated_1.json";

    [System.Serializable]
    public class LibraryRowUI
    {
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI cellCountText;
        public Button viewPatternButton;
    }

    [Header("UI Rows (Size = 35)")]
    [SerializeField]
    private List<LibraryRowUI> rows = new List<LibraryRowUI>();

    [SerializeField]
    private GameObject libraryRowPrefab;

    [SerializeField]
    private Transform contentParent;

    private LibraryTable table;

    private void OnEnable()
    {
        table = LoadLibrary();

        if (table == null || table.entries == null)
            return;

        PopulateLibraryTable();
    }

    private void PopulateLibraryTable()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var entry in table.entries)
        {
            GameObject rowGO = Instantiate(libraryRowPrefab, contentParent);
            LibraryRowController rowController = rowGO.GetComponent<LibraryRowController>();
            rowController.Bind(entry);
        }
    }

    private void BindRow(LibraryRowUI row, LibraryEntry entry)
    {
        row.nameText.text = entry.name;
        row.cellCountText.text = entry.cellCount.ToString();

        row.viewPatternButton.interactable = true;
        row.viewPatternButton.onClick.RemoveAllListeners();

        var rowController = row.viewPatternButton.GetComponent<LibraryRowController>();
        rowController.Bind(entry);

        row.viewPatternButton.onClick.AddListener(rowController.OnViewPatternClicked);
    }

    private LibraryTable LoadLibrary()
    {
        string path = Path.Combine(Application.streamingAssetsPath, LibraryFileName);
        string json;

    #if UNITY_ANDROID && !UNITY_EDITOR
        using (var request = UnityEngine.Networking.UnityWebRequest.Get(path))
        {
            request.SendWebRequest();
            while (!request.isDone) { }

            if (request.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to load library: {request.error}");
                return null;
            }

            json = request.downloadHandler.text;
        }
    #else
        json = File.ReadAllText(path);
    #endif

        LibraryTable table = JsonUtility.FromJson<LibraryTable>(json);

        if (table == null || table.entries == null)
        {
            Debug.LogError("Failed to deserialize library table or entries.");
            return null;
        }

        // ✅ SORT AFTER DERIVED FIELDS EXIST
        table.entries.Sort((a, b) =>
        {
            int cellCompare = a.cellCount.CompareTo(b.cellCount);
            if (cellCompare != 0)
                return cellCompare;

            return string.Compare(a.name, b.name, System.StringComparison.OrdinalIgnoreCase);
        });

        return table;
    }
}