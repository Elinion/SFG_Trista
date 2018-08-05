using System.Collections;
using System.Collections.Generic;
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
        LevelData levelData = GameController.instance.levelData;
        levelName.text = levelData.levelName;
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].sprite = ColorManager.instance.getColorAssets(levelData.tiles[i].color).tile;
        }
    }

}
