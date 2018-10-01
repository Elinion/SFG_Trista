using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public Message tutorialMessage;
    public GameObject hiddenButton;

    private GameObject playerControls;

    private void Start() {
        playerControls = GameObject.FindGameObjectWithTag(Tags.Player);
        disablePlayerControls();
        showTutorialMessageAfterTime(.5f);
    }

    public void EndTutorial() {
        tutorialMessage.close();
        hiddenButton.SetActive(false);
        playerControls.SetActive(true);
    }

    private void disablePlayerControls() {
        playerControls.SetActive(false);
    }

    private void showTutorialMessageAfterTime(float seconds) {
        StartCoroutine(showTutorialMessage(seconds));
    }

    private IEnumerator showTutorialMessage(float seconds) {
        yield return new WaitForSeconds(seconds);
        tutorialMessage.open();
    }
}