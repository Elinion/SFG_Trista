using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour {
    public GameObject tutorialMessage;

    private GameObject playerControls;

    private void Start() {
        playerControls = GameObject.FindGameObjectWithTag(Tags.Player);
        disablePlayerControls();
        hideTutorialMessage();
        showTutorialMessageAfterTime(.5f);
    }

    public void EndTutorial() {
        hideTutorialMessage();
        playerControls.SetActive(true);
    }

    private void disablePlayerControls() {
        playerControls.SetActive(false);
    }

    private void hideTutorialMessage() {
        tutorialMessage.SetActive(false);
    }

    private void showTutorialMessageAfterTime(float seconds) {
        StartCoroutine(showTutorialMessage(seconds));
    }

    private IEnumerator showTutorialMessage(float seconds) {
        yield return new WaitForSeconds(seconds);
        tutorialMessage.SetActive(true);
    }
}