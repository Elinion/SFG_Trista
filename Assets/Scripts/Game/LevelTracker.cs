using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTracker : MonoBehaviour {

    public Image topLeftTile;
    public Image topMiddleTile;
    public Image topRightTile;
    public Image middleLeftTile;
    public Image middleCenterTile;
    public Image middleRightTile;
    public Image bottomLeftTile;
    public Image bottomMiddleTile;
    public Image bottomRightTile;

	void Start () {
        SetLevelData();
	}

    private void SetLevelData() {
        LevelData level = GameController.instance.Level;
        topLeftTile.sprite = ColorManager.instance.GetColorAssets(level.topLeftTile.color).tile;
        topMiddleTile.sprite = ColorManager.instance.GetColorAssets(level.topMiddleTile.color).tile;
        topRightTile.sprite = ColorManager.instance.GetColorAssets(level.topRightTile.color).tile;
        middleLeftTile.sprite = ColorManager.instance.GetColorAssets(level.middleLeftTile.color).tile;
        middleCenterTile.sprite = ColorManager.instance.GetColorAssets(level.middleCenterTile.color).tile;
        middleRightTile.sprite = ColorManager.instance.GetColorAssets(level.middleRightTile.color).tile;
        bottomLeftTile.sprite = ColorManager.instance.GetColorAssets(level.bottomLeftTile.color).tile;
        bottomMiddleTile.sprite = ColorManager.instance.GetColorAssets(level.bottomMiddleTile.color).tile;
        bottomRightTile.sprite = ColorManager.instance.GetColorAssets(level.bottomRightTile.color).tile;
    }
	
}
