using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelThumbnail : MonoBehaviour {

    public Text levelName;
    public List<Image> tiles = new List<Image>();

    private LevelData levelData;

    public void setLevelData(LevelData levelData) {
        this.levelData = levelData;
        levelName.text = levelData.levelName;
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].sprite = ColorManager.instance.getColorAssets(levelData.tiles[i].color).tile;
        }
    }

    public void Play() {
        GameController.instance.playLevel(levelData);
    }
}
