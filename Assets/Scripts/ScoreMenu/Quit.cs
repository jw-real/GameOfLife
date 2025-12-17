using UnityEngine;
using System.IO;

public class Quit : MonoBehaviour
{
    // OnClick Quit Game and destroy runtime_progession.json
    private const string ProgressionFileName = "runtime_progression.json";

    public void OnQuitButtonPressed()
    {
        DeleteProgressionFile();
        QuitApplication();
    }

    private void DeleteProgressionFile()
    {
        string filePath = Path.Combine(
            Application.persistentDataPath,
            ProgressionFileName
        );

        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Debug.Log($"Deleted progression file at: {filePath}");
            }
            catch (IOException e)
            {
                Debug.LogError($"Failed to delete progression file: {e.Message}");
            }
        }
        else
        {
            Debug.Log("No progression file found to delete.");
        }
    }

    private void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
