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
        if (table == null)
            return;

        PopulateLibraryTable();
    }

    private void PopulateLibraryTable()
    {
        LibraryTable table = LoadLibrary();

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
        // Android requires UnityWebRequest
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

        return JsonUtility.FromJson<LibraryTable>(json);
    }

}
