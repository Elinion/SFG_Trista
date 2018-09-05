using GameData;
using UnityEngine;
using UnityEngine.UI;

public class LevelGroupIcon : MonoBehaviour {
    public Text levelGroupName;
    public Text levelGroupState;

    private LevelGroup levelGroup;

    public void setLevelGroup(LevelGroup levelGroup) {
        this.levelGroup = levelGroup;
        levelGroupName.text = levelGroup.name;
        levelGroupState.text = SavedGameController.instance.getLevelGroupState(levelGroup).ToString();
    }

    public void select() {
        GameController.instance.selectLevelGroup(levelGroup);
        GameController.instance.goToLevelSelectionMenu();
    }
}