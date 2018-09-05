using System.Collections;
using System.Collections.Generic;
using GameProgress;
using UnityEngine;
using UnityEngine.UI;
using Level = GameData.Level;

public class LevelThumbnail : MonoBehaviour {

    public Text levelName;
    public List<Image> tiles = new List<Image>();
    public Text levelProgress;

    private Level level;

    public void setLevel(Level level) {
        this.level = level;
        levelName.text = level.name;
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].sprite = ColorManager.instance.getColorAssets(level.tiles[i].color).tile;
        }

        State levelState = SavedGameController.instance.getLevelState(level);
        levelProgress.text = levelState.ToString();
    }

    public void Play() {
        GameController.instance.playLevel(level);
    }
}
