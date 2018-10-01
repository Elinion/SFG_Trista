using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {
    void Awake() {
        SceneManager.LoadScene(Scenes.Persistent, LoadSceneMode.Additive);
    }

    void Start() {
        loadUserProgress();
    }

    private void loadUserProgress() {
//        Social.localUser.Authenticate(isAuthenticated => {
//            SavedGameController.instance.loadGameProgress();
//            goToNextScreen();
//        });
        SavedGameController.instance.loadGameProgress();
        goToNextScreen();
    }

    private void goToNextScreen() {
        if (LocalizationController.instance.isSavedLanguage()) {
            LocalizationController.instance.loadSavedLanguage();
            SceneManager.LoadSceneAsync(Scenes.MainMenu);
        } else {
            SceneManager.LoadSceneAsync(Scenes.LanguageSelectionMenu);
        }
    }
}