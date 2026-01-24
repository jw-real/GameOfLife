using UnityEngine;
using System.IO;

public class SessionBootstrap : MonoBehaviour
{
    private const string ProfileFileName = "player_profile.json";
    [SerializeField] private GameObject libraryAccessIcon;

    void Awake()
    {
        InitializeProfile();
    }

    void InitializeProfile()
    {
        string path = Path.Combine(
            Application.persistentDataPath,
            ProfileFileName
        );

        // Always reset profile each session
        PlayerProfileData profile = new PlayerProfileData();

        string json = JsonUtility.ToJson(profile, true);
        File.WriteAllText(path, json);

        Debug.Log("Session profile initialized:\n" + json);
    }

    void InitializeLibraryButtons()
    {
        libraryAccessIcon.SetActive(false);
    }
}