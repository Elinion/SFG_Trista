using System.Collections;
using GameProgress;
using UnityEngine;
using Level = GameData.Level;
using LevelGroup = GameData.LevelGroup;

public class LevelEnd : MonoBehaviour {
    public GameObject levelEndUi;
    public LevelEndMessage levelEndMessage;

    public void goToNextLevel() {
        Debug.Log("go to next level");
        if (GameController.instance.isNextLevel()) {
            GameController.instance.goToNextLevel();
        } else {
            GameController.instance.goToLevelGroupSelectionMenu();
        }
    }

    public void goToNextLevelGroup() {
        GameController.instance.selectNextLevelGroup();
        GameController.instance.goToLevelSelectionMenu();
    }

    public void triggerEnd(State state) {
        Level currentLevel = GameController.instance.level;
        SavedGameController.instance.updateLevelState(currentLevel, state);
        StartCoroutine(showEnd(state));
    }

    private IEnumerator showEnd(State state) {
        const float waitForSeconds = 1f;
        yield return new WaitForSeconds(waitForSeconds);

        levelEndUi.SetActive(true);
        levelEndMessage.show(state);

        if (state == State.NotOk) yield break;

        yield return new WaitForSeconds(3 * waitForSeconds);
        goToNextLevel();
    }
}