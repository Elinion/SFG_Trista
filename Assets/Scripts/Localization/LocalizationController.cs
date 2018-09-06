using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationController : MonoBehaviour {
    public static LocalizationController instance;

    private readonly Dictionary<Language, string> languageFiles = new Dictionary<Language, string> {
        {Language.English, "localizationText_en.json"},
        {Language.French, "localizationText_fr.json"}
    };

    private Dictionary<string, string> localizedText;
    private const string LanguagePref = "Language";
    private const string MissingTextString = "Localized text not found";

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public bool isSavedLanguage() {
        return PlayerPrefs.HasKey(LanguagePref);
    }

    public void loadSavedLanguage() {
        Language savedLanguage = (Language) Enum.Parse(typeof(Language), PlayerPrefs.GetString(LanguagePref));
        loadLanguage(savedLanguage);
    }

    public void deleteSavedLanguage() {
        PlayerPrefs.DeleteKey(LanguagePref);
    }

    public void loadLanguage(Language language) {
        loadLocalizedText(languageFiles[language]);
        PlayerPrefs.SetString(LanguagePref, language.ToString());
    }

    private void loadLocalizedText(string fileName) {
        LocalizationData loadedData = Files.readFromJson<LocalizationData>(fileName);
        localizedText = convertLocalizationDataToDictionary(loadedData);
    }

    private Dictionary<string, string> convertLocalizationDataToDictionary(LocalizationData localizationData) {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach (LocalizationItem item in localizationData.items) {
            dictionary.Add(item.key, item.value);
        }

        return dictionary;
    }

    public string getLocalizedValue(string key) {
        if (localizedText.ContainsKey(key)) {
            return localizedText[key];
        }

        Debug.Log("Missing localized text: " + key);
        return key;
    }
}