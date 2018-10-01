using System;
using System.Collections.Generic;
using GameData;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public static GameController instance;

    public Level level { get; private set; }

    private int selectedWorld;
    private int selectedLevelGroup;
    private int selectedLevel;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void goToLanguageSelection() {
        SceneManager.LoadScene(Scenes.LanguageSelectionMenu);
    }

    public void goToLevelGroupSelectionMenu() {
        SceneManager.LoadScene(Scenes.LevelGroupSelectionMenu);
    }

    public void goToLevelSelectionMenu() {
        SceneManager.LoadScene(Scenes.LevelSelectionMenu);
    }

    public void goToMainMenu() {
        SceneManager.LoadScene(Scenes.MainMenu);
    }

    public void goToMainMenuAndSkipAnimation() {
        MainMenu.playAnimation = false;
        SceneManager.LoadScene(Scenes.MainMenu);
    }

    public void goToNextLevel() {
        int nextLevelId = selectedLevel + 1;
        playLevel(getLevelGroup().levels[nextLevelId]);
    }
    
    public void selectLevelGroup(LevelGroup levelGroup) {
        selectedLevelGroup = levelGroup.index;
    }

    public void selectNextLevelGroup() {
        selectedLevelGroup++;
    }

    public void selectPreviousLevelGroup() {
        selectedLevelGroup = Mathf.Max(0, selectedLevelGroup - 1);
    }

    public LevelGroup getNextLevelGroup() {
        return ByIndexAccessor.getLevelGroup(selectedWorld, selectedLevelGroup + 1);
    }

    public bool isNextLevel() {
        return ByIndexAccessor.getLevel(selectedWorld, selectedLevelGroup, selectedLevel + 1) != null;
    }

    public Level getLevel() {
        return ByIndexAccessor.getLevel(selectedWorld, selectedLevelGroup, selectedLevel);
    }

    public LevelGroup getLevelGroup() {
        return ByIndexAccessor.getLevelGroup(selectedWorld, selectedLevelGroup);
    }

    public bool isNextLevelGroup() {
        return ByIndexAccessor.getLevelGroup(selectedWorld, selectedLevelGroup + 1) != null;
    }

    public bool isPreviousLevelGroup() {
        return ByIndexAccessor.getLevelGroup(selectedWorld, selectedLevelGroup - 1) != null;
    }

    public World getWorld() {
        return ByIndexAccessor.getWorld(selectedWorld);
    }

    public void playLevel(Level level) {
        setLevel(level);
        SceneManager.LoadScene(Scenes.Game);

        if (!string.IsNullOrEmpty(level.sceneAdditiveName)) {
            SceneManager.LoadScene(level.sceneAdditiveName, LoadSceneMode.Additive);
        }
    }

    private void setLevel(Level level) {
        this.level = level;
        selectedLevel = level.index;
    }


    public void reloadScene() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}