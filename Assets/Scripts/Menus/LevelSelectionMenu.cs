using System;
using System.Collections;
using System.Collections.Generic;
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
        LevelGroupData levelGroupData = getLevelGroups()[GlobalData.selectedLevelGroupId];
        levelGroupName.text = levelGroupData.levelGroupName;

        for (int i = 0; i < levelGroupData.levels.Length; i++) {
            levelPreviews[i].gameObject.SetActive(true);
            levelPreviews[i].setLevelData(levelGroupData.levels[i]);
        }
    }

    public void GoToNextPage() {
        int nextLevelGroupId = (GlobalData.selectedLevelGroupId + 1) % getLevelGroups().Length;
        setLevelGroupId(nextLevelGroupId);
        createCurrentLevelGroupPreviews();
    }

    public void GoToPreviousPage() {
        if (GlobalData.selectedLevelGroupId == 0) {
            goToLastPage();
            return;
        }

        int previousLevelGroupId = GlobalData.selectedLevelGroupId - 1;
        setLevelGroupId(previousLevelGroupId);
        createCurrentLevelGroupPreviews();
    }

    private void goToLastPage() {
        int lastLevelGroupId = getLevelGroups().Length - 1;
        setLevelGroupId(lastLevelGroupId);
        createCurrentLevelGroupPreviews();
    }

    private void setLevelGroupId(int levelGroupId) {
        GlobalData.selectedLevelGroupId = levelGroupId;
    }

    private void setLevelWorldId(int levelWorldId) {
        GlobalData.selectedWorldId = levelWorldId;
    }

    private LevelGroupData[] getLevelGroups() {
        return DataController.instance
            .getWorldById(GlobalData.selectedWorldId)
            .levelGroups;
    }
}