using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public static GameController instance;

    public LevelData levelData { get; private set; }

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this)
        {
            Destroy (gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void GoToLevelMenu() {
        SceneManager.LoadSceneAsync("levelSelectionMenu");
    }

    public void GoToMainMenu() {
        SceneManager.LoadSceneAsync("mainMenu");
    }

    public void goToNextLevelGroup() {
        GlobalData.selectedLevelGroupId++;
        GoToLevelMenu();
    }

    public bool isNextLevelGroup() {
        WorldData worldData = DataController.instance.getWorldById(GlobalData.selectedWorldId);
        int nextLevelGroupId = GlobalData.selectedLevelGroupId + 1;
        return nextLevelGroupId < worldData.levelGroups.Length;
    }

    public void goToNextLevel() {
        int nextLevelId = GlobalData.selectedLevelId + 1;
        playLevel(getCurrentLevelGroupData().levels[nextLevelId]);
    }

    public bool isNextLevel() {
        LevelGroupData currentLevelGroupData = getCurrentLevelGroupData();
        int nextLevelId = GlobalData.selectedLevelId + 1;
        return nextLevelId < currentLevelGroupData.levels.Length;
    }

    private LevelGroupData getCurrentLevelGroupData() {
        return DataController.instance
            .getWorldById(GlobalData.selectedWorldId)
            .levelGroups[GlobalData.selectedLevelGroupId];
    }

    public void playLevel(LevelData levelData) {
        setLevelData(levelData);
        SceneManager.LoadScene("game");

        if (!string.IsNullOrEmpty(levelData.sceneAdditiveName)) {
            SceneManager.LoadScene(levelData.sceneAdditiveName, LoadSceneMode.Additive);
        }
    }

    private void setLevelData(LevelData levelData) {
        this.levelData = levelData;
        GlobalData.selectedLevelId = levelData.levelId;
    }


    public void reloadScene() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}