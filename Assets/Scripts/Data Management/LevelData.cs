using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData {

    public string levelName;
    public ColorGeneratorData[] tubeSequence;
    public ColorManager.Colors[] possibleColors;

    public TileData topLeftTile;
    public TileData topMiddleTile;
    public TileData topRightTile;
    public TileData middleLeftTile;
    public TileData middleCenterTile;
    public TileData middleRightTile;
    public TileData bottomLeftTile;
    public TileData bottomMiddleTile;
    public TileData bottomRightTile;
}
