using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;
using UnityEngine.UI;

public class LevelTracker : MonoBehaviour {
    public Text levelName;
    public List<Image> tiles = new List<Image>();

    private void Start()
    {
        setLevelData();
    }

    private void setLevelData()
    {
        Level levelData = GameController.instance.level;
        levelName.text = levelData.name;
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].sprite = ColorManager.instance.getColorAssets(levelData.tiles[i].color).tile;
        }
    }

}
