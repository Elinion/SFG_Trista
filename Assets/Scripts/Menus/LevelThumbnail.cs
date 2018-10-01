using System.Collections.Generic;
using GameProgress;
using UnityEngine;
using UnityEngine.UI;
using Level = GameData.Level;

public class LevelThumbnail : MonoBehaviour {
    public Text levelName;
    public List<Image> tiles = new List<Image>();
    public GameObject levelProgressOk;
    public GameObject levelProgressPerfect;

    private Level level;

    private void Awake() {
        levelProgressOk.SetActive(false);
        levelProgressPerfect.SetActive(false);
    }

    public void setLevel(Level level) {
        this.level = level;
        levelName.text = level.name;
        for (int i = 0; i < tiles.Count; i++) {
            tiles[i].GetComponent<Image>().color = ColorManager.instance.getColorAssets(level.tiles[i].color).color;
        }

        State levelState = SavedGameController.instance.getLevelState(level);

        levelProgressOk.SetActive(levelState == State.Ok);
        levelProgressPerfect.SetActive(levelState == State.Perfect);
    }

    public void Play() {
        GameController.instance.playLevel(level);
    }
}