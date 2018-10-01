using System;
using System.Collections.Generic;
using GameProgress;
using UnityEngine;

public class LevelEndMessage : MonoBehaviour {
    public GameObject notOkLogo;
    public GameObject notOkMessage;
    public GameObject okLogo;
    public GameObject perfectLogo;

    public List<GameObject> okMessages;
    public List<GameObject> okStars;
    public List<GameObject> perfectMessages;
    public List<GameObject> perfectStars;

    private void hideMessagesAndStars() {
        okMessages.ForEach(hideGameObject());
        okStars.ForEach(hideGameObject());
        perfectMessages.ForEach(hideGameObject());
        perfectStars.ForEach(hideGameObject());
        notOkLogo.SetActive(false);
        notOkMessage.SetActive(false);
        okLogo.SetActive(false);
        perfectLogo.SetActive(false);
    }

    private static Action<GameObject> hideGameObject() {
        return go => go.SetActive(false);
    }

    public void show(string message) {
        hideMessagesAndStars();
    }

    public void show(State levelState) {
        hideMessagesAndStars();
        int levelStars = GameController.instance.getLevel().stars;
        switch (levelState) {
            case State.NotOk:
                notOkLogo.SetActive(true);
                notOkMessage.SetActive(true);
                break;
            case State.Ok:
                okLogo.SetActive(true);
                okMessages[levelStars].SetActive(true);
                okStars[levelStars].SetActive(true);
                break;
            case State.Perfect:
                perfectLogo.SetActive(true);
                perfectMessages[levelStars].SetActive(true);
                perfectStars[levelStars].SetActive(true);
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }
}