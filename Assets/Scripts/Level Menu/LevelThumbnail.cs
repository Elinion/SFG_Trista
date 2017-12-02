using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelThumbnail : MonoBehaviour {

    public Text levelName;

    private LevelData levelData;

    public void SetLevelData(LevelData levelData) {
        this.levelData = levelData;
        levelName.text = levelData.levelName;
    }

    public void Play() {
        GameController.instance.PlayLevel(levelData);
    }
}
