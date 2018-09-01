using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour {
    public GameObject levelOk;
    public GameObject levelPerfect;
    public GameObject levelNotOk;
    public GameObject levelGroupEnd;

    private void Start() {
        hideAllLevelEnds();
    }

    public void goToNextLevel() {
        if (GameController.instance.isNextLevel()) {
            GameController.instance.goToNextLevel();
        } else if (GameController.instance.isNextLevelGroup()) {
            levelGroupEnd.SetActive(true);
            LevelGroup nextLevelGroupData = GameController.instance.getNextLevelGroup();
            LevelGroupEnd levelGroupEndScript = levelGroupEnd.GetComponent<LevelGroupEnd>();
            levelGroupEndScript.unlockedLevelGroupName.text = nextLevelGroupData.name;
        } else {
            Debug.Log("Not implemented: unlock next world");
        }
    }

    public void goToNextLevelGroup() {
        GameController.instance.selectNextLevelGroup();
        GameController.instance.goToLevelSelectionMenu();
    }

    private void hideAllLevelEnds() {
        levelOk.SetActive(false);
        levelPerfect.SetActive(false);
        levelNotOk.SetActive(false);
        levelGroupEnd.SetActive(false);
    }

    public void triggerEnd(GameProgress.State state) {
        Level currentLevel = GameController.instance.level;
        SavedGameController.instance.updateLevelState(currentLevel, state);
        showEnd(state);
    }
    
    private void showEnd(GameProgress.State state) {
        const float waitForSeconds = 1f;
        switch (state) {
            case GameProgress.State.Ok:
                StartCoroutine(showOkEnd(waitForSeconds));
                break;
            case GameProgress.State.Perfect:
                StartCoroutine(showPerfectEnd(waitForSeconds));
                break;
            case GameProgress.State.NotOk:
                StartCoroutine(showNotOkEnd(waitForSeconds));
                break;
        }
    }

    private IEnumerator showOkEnd(float waitForSeconds) {
        yield return new WaitForSeconds(waitForSeconds);
        levelOk.SetActive(true);
        yield return new WaitForSeconds(2 * waitForSeconds);
        goToNextLevel();
    }

    private IEnumerator showPerfectEnd(float waitForSeconds) {
        yield return new WaitForSeconds(waitForSeconds);
        levelPerfect.SetActive(true);
        yield return new WaitForSeconds(2 * waitForSeconds);
        goToNextLevel();
    }

    private IEnumerator showNotOkEnd(float waitForSeconds) {
        yield return new WaitForSeconds(waitForSeconds);
        levelNotOk.SetActive(true);
    }
}