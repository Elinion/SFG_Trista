using GameData;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionMenu : MonoBehaviour {
    public Text levelGroupName;
    public LevelThumbnail[] levelPreviews = new LevelThumbnail[10];

    void Start() {
        initLevelPreviews();
        createCurrentLevelGroupPreviews();
    }

    private void initLevelPreviews() {
        foreach (LevelThumbnail level in levelPreviews) {
            level.gameObject.SetActive(false);
        }
    }

    private void createCurrentLevelGroupPreviews() {
        LevelGroup levelGroupData = GameController.instance.getLevelGroup();
        levelGroupName.text = levelGroupData.name;

        for (int i = 0; i < levelGroupData.levels.Length; i++) {
            levelPreviews[i].gameObject.SetActive(true);
            levelPreviews[i].setLevel(levelGroupData.levels[i]);
        }
    }

    public void goToNextPage() {
        GameController.instance.goToNextLevel();
        createCurrentLevelGroupPreviews();
    }

    public void goToPreviousPage() {
        GameController.instance.selectPreviousLevelGroup();
        createCurrentLevelGroupPreviews();
    }
}