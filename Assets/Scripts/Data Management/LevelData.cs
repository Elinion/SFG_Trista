using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData {

    public int levelId;
    public string levelName;
    public bool shiftTubesAfterPlay;
    public bool enableTileGrowth;
    public ColorManager.Colors[] startingTubeColors;
    public ColorGeneratorData[] tubeSequence;
    public ColorManager.Colors[] possibleColors;
    public TileData[] tiles;
    public string sceneAdditiveName;
}
