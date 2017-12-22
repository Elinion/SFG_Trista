using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData {

    public string levelName;
    public ColorGeneratorData[] tubeSequence;
    public ColorManager.Colors[] possibleColors;
    public TileData[] tiles;
}
