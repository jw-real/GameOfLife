using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LibraryManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject rowPrefab;

    [Header("Data")]
    [SerializeField] private string libraryFileName = "pattern_library_truncated_1.json";

    private void OnEnable()
    {
        Debug.Log("[LibraryManager] OnEnable called");
        StartCoroutine(LoadLibraryCoroutine());
    }

    private IEnumerator LoadLibraryCoroutine()
    {
        string path = Path.Combine(Application.streamingAssetsPath, libraryFileName);

#if UNITY_EDITOR || UNITY_STANDALONE
        path = "file://" + path;
#endif

        Debug.Log($"[LibraryManager] StreamingAssetsPath: {Application.streamingAssetsPath}");
        Debug.Log($"[LibraryManager] Full path exists check: {path}");
        Debug.Log($"[LibraryManager] Requesting library from: {path}");

        using (UnityWebRequest request = UnityWebRequest.Get(path))
        {
            Debug.Log($"[LibraryManager] Application.streamingAssetsPath = {Application.streamingAssetsPath}");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[LibraryManager] Failed to load library: {request.error}");
                yield break;
            }

            string json = request.downloadHandler.text;

            Debug.Log($"[LibraryManager] Library JSON loaded ({json.Length} chars)");

            LibraryWrapper wrapper;
            try
            {
                wrapper = JsonUtility.FromJson<LibraryWrapper>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[LibraryManager] JSON Parse Exception: {e.Message}");
                yield break;
            }

            if (wrapper == null || wrapper.entries == null)
            {
                Debug.LogError("[LibraryManager] Parsed library is null or missing entries");
                yield break;
            }

            Debug.Log($"[LibraryManager] Parsed {wrapper.entries.Count} library entries");

            PopulateLibraryTable(wrapper.entries);
        }
    }

    private void PopulateLibraryTable(List<LibraryEntry> entries)
    {
        Debug.Log("[LibraryManager] Populating library table");

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (LibraryEntry entry in entries)
        {
            GameObject rowGO = Instantiate(rowPrefab, contentParent);
            LibraryRowController controller = rowGO.GetComponent<LibraryRowController>();

            if (controller == null)
            {
                Debug.LogError("[LibraryManager] Row prefab missing LibraryRowController");
                continue;
            }

            controller.Bind(entry);
        }

        Debug.Log("[LibraryManager] Library table population complete");
    }

    // JSON wrapper for JsonUtility
    [System.Serializable]
    private class LibraryWrapper
    {
        public List<LibraryEntry> entries;
    }
}