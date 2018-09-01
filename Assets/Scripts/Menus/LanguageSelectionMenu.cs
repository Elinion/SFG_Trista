using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageSelectionMenu : MonoBehaviour {

    public void loadEnglish() {
        load(Language.English);
    }

    public void loadFrench() {
        load(Language.French);
    }

    public void goBack() {
        SceneManager.LoadScene(Scenes.MainMenu);
    }
    
    private void load(Language language) {
        LocalizationController.instance.loadLanguage(language);
    }
}