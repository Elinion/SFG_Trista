using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTracker : MonoBehaviour
{
    public List<Image> tiles = new List<Image>();

    void Start()
    {
        SetLevelData();
    }

    private void SetLevelData()
    {
        LevelData level = GameController.instance.Level;
        if (tiles.Count != level.tiles.Length)
        {
            Debug.Log("LevelTracker::SetLevelData: the number of tile thumbnails and level tiles don't match.");
            return;
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].sprite = ColorManager.instance.GetColorAssets(level.tiles[i].color).tile;
        }
    }

}
