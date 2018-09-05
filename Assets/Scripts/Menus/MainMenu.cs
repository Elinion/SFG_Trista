using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public static bool playAnimation = true;

    public GameObject logo;
    public GameObject screenFadeIn;
    public GameObject buttonsFadeIn;

    private Animator animator;

    private void Awake() {
        SceneManager.LoadScene(Tags.Persistent, LoadSceneMode.Additive);
        animator = GetComponent<Animator>();
    }

    private void Start() {
        if (playAnimation) return;

        skipAnimation();
        playAnimation = true;
    }

    private void skipAnimation() {
        logo.GetComponent<Animation>().enabled = false;
        logo.SetActive(true);
        screenFadeIn.GetComponent<Animator>().enabled = false;
        screenFadeIn.SetActive(false);
        animator.enabled = false;
        buttonsFadeIn.SetActive(false);
    }

    public void quit() {
        Application.Quit();
    }

    public void goToLevels() {
        GameController.instance.goToLevelGroupSelectionMenu();
    }

    public void goToSurvivalMode() {
        SceneManager.LoadSceneAsync("survivalMode");
    }

    public void goToSettings() {
        LocalizationController.instance.deleteSavedLanguage();
        GameController.instance.goToLanguageSelection();
    }
}