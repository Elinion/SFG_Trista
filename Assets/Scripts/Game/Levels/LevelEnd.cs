using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        } else {
            levelGroupEnd.SetActive(true);

            WorldData worldData = DataController.instance.getWorldById(GlobalData.selectedWorldId);
            int nextLevelGroupId = GlobalData.selectedLevelGroupId + 1;
            LevelGroupData nextLevelGroupData = worldData.levelGroups[nextLevelGroupId];

            LevelGroupEnd levelGroupEndScript = levelGroupEnd.GetComponent<LevelGroupEnd>();
            levelGroupEndScript.unlockedLevelGroupName.text = nextLevelGroupData.levelGroupName;
        }
    }

    public void goToNextLevelGroup() {
        if (GameController.instance.isNextLevelGroup()) {
            GameController.instance.goToNextLevelGroup();
        } else {
            Debug.Log("Not implemented: unlock next world");
        }
    }

    private void hideAllLevelEnds() {
        levelOk.SetActive(false);
        levelPerfect.SetActive(false);
        levelNotOk.SetActive(false);
        levelGroupEnd.SetActive(false);
    }

    public void showEnd(Level.Progress levelProgress) {
        const float waitForSeconds = 1f;
        switch (levelProgress) {
            case Level.Progress.Ok:
                StartCoroutine(showOkEnd(waitForSeconds));
                break;
            case Level.Progress.Perfect:
                StartCoroutine(showPerfectEnd(waitForSeconds));
                break;
            case Level.Progress.NotOk:
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