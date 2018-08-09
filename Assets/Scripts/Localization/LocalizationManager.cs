using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class LocalizationManager : MonoBehaviour {
    public static LocalizationManager instance;

    private Dictionary<string, string> localizedText;
    private const string MissingTextString = "Localized text not found";
    public const string SelectedLocalizationText = "SelectedLocalizationText";

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        skipLanguageSelectionIfAlreadySelected();
    }

    private void skipLanguageSelectionIfAlreadySelected() {
        string selectedLocalizationText = PlayerPrefs.GetString(SelectedLocalizationText);
        if (string.IsNullOrEmpty(selectedLocalizationText)) return;

        loadLocalizedText(selectedLocalizationText);
    }

    public void loadLocalizedText(string fileName) {
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath)) {
            string dataAsJson = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            foreach (LocalizationItem item in loadedData.items) {
                localizedText.Add(item.key, item.value);
            }

            PlayerPrefs.SetString(SelectedLocalizationText, fileName);
            Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
            SceneManager.LoadScene("MainMenu");
        } else {
            Debug.LogError("Cannot find file: " + filePath);
        }
    }

    public string getLocalizedValue(string key) {
        string result = MissingTextString;
        if (localizedText.ContainsKey(key)) {
            result = localizedText[key];
        }

        return result;
    }
}