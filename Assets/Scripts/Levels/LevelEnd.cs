using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {
    public GameObject levelOk;
    public GameObject levelPerfect;
    public GameObject levelNotOk;

    private void Start() {
        hideAllLevelEnds();
    }

    private void hideAllLevelEnds() {
        levelOk.SetActive(false);
        levelPerfect.SetActive(false);
        levelNotOk.SetActive(false);
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
    }

    private IEnumerator showPerfectEnd(float waitForSeconds) {
        yield return new WaitForSeconds(waitForSeconds);
        levelPerfect.SetActive(true);
    }

    private IEnumerator showNotOkEnd(float waitForSeconds) {
        yield return new WaitForSeconds(waitForSeconds);
        levelNotOk.SetActive(true);
    }
}