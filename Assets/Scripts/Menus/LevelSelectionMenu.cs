using System.Collections.Generic;
using GameData;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionMenu : MonoBehaviour {
    public Text levelGroupName;
    public LevelThumbnail[] levelPreviews = new LevelThumbnail[10];
    public GameObject nextLevelGroupButton;
    public GameObject previousLevelGroupButton;

    void Awake() {
        hideLevelPreviews();
    }

    void Start() {
        setUpUI();
    }

    private void hideLevelPreviews() {
        foreach (LevelThumbnail level in levelPreviews) {
            level.gameObject.SetActive(false);
        }
    }

    private void setUpUI() {
        LevelGroup levelGroup = GameController.instance.getLevelGroup();
        levelGroupName.text = levelGroup.name;
        setUpLevelPreviews(levelGroup.levels);
        nextLevelGroupButton.SetActive(GameController.instance.isNextLevelGroup());
        previousLevelGroupButton.SetActive(GameController.instance.isPreviousLevelGroup());
    }

    private void setUpLevelPreviews(IList<Level> levels) {
        for (int i = 0; i < levels.Count; i++) {
            levelPreviews[i].gameObject.SetActive(true);
            levelPreviews[i].setLevel(levels[i]);
        }
    }

    public void goBackToLevelGroupSelectionMenu() {
        GameController.instance.goToLevelGroupSelectionMenu();
    }

    public void goToNextPage() {
        GameController.instance.selectNextLevelGroup();
        setUpUI();
    }

    public void goToPreviousPage() {
        GameController.instance.selectPreviousLevelGroup();
        setUpUI();
    }
}