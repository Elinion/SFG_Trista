using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [System.Serializable]
    public class Line
    {
        public Tile firstTile;
        public Tile secondTile;
        public Tile thirdTile;

        private readonly List<Tile> allTiles = new List<Tile>();
        public List<Tile> AllTiles
        {
            get {
                if(allTiles.Count == 0) {
                    SetUpLines();
                }
                return allTiles; 
            }
        }

        private void SetUpLines()
        {
            allTiles.Add(firstTile);
            allTiles.Add(secondTile);
            allTiles.Add(thirdTile);
        }
    }

    public Tile topLeftTile;
    public Tile topMiddleTile;
    public Tile topRightTile;
    public Tile middleLeftTile;
    public Tile middleCenterTile;
    public Tile middleRightTile;
    public Tile bottomLeftTile;
    public Tile bottomMiddleTile;
    public Tile bottomRightTile;
    public List<Line> lines = new List<Line>();

    private GameController gameController;
    private readonly List<Tile> allTiles = new List<Tile>();

    public List<Tile> GetAlignedTiles()
    {
        List<Tile> alignedTiles = new List<Tile>();
        lines.ForEach(line =>
        {
            if (line.firstTile.Color != ColorManager.Colors.None
                && line.firstTile.Color != ColorManager.Colors.Gray
                && line.firstTile.Color == line.secondTile.Color
                && line.firstTile.Color == line.thirdTile.Color)
            {
                AddRangeToListWithoutDuplicates(alignedTiles, line.AllTiles);
            }
        });
        return alignedTiles;
    }

    public void HideTileHints()
    {
        allTiles.ForEach(tile => tile.HideHints());
    }

    public bool IsLevelCompleted(LevelData level)
    {
        return DoesTileMatch(topLeftTile, level.topLeftTile)
            && DoesTileMatch(topMiddleTile, level.topMiddleTile)
            && DoesTileMatch(topRightTile, level.topRightTile)
            && DoesTileMatch(middleLeftTile, level.middleLeftTile)
            && DoesTileMatch(middleCenterTile, level.middleCenterTile)
            && DoesTileMatch(middleRightTile, level.middleRightTile)
            && DoesTileMatch(bottomLeftTile, level.bottomLeftTile)
            && DoesTileMatch(bottomMiddleTile, level.bottomMiddleTile)
            && DoesTileMatch(bottomRightTile, level.bottomRightTile);
    }

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>();
        SetTiles();
    }

    private void Start()
    {
        InitBoard();
    }

    private bool DoesTileMatch(Tile tile, TileData tileData)
    {
        if (tileData.isRequired)
        {
            return tileData.color == tile.Color
                           && tileData.minimumValue <= tile.Level;
        }
        return true;
    }

    private void AddRangeToListWithoutDuplicates(List<Tile> listToAddTo, List<Tile> whatToAdd)
    {
        whatToAdd.ForEach(tile =>
        {
            if (!listToAddTo.Contains(tile))
            {
                listToAddTo.Add(tile);
            }
        });
    }

    private void InitBoard()
    {
        LevelData level = gameController.Level;
        topLeftTile.Color = level.topLeftTile.defaultColor;
        topMiddleTile.Color = level.topMiddleTile.defaultColor;
        topRightTile.Color = level.topRightTile.defaultColor;
        middleLeftTile.Color = level.middleLeftTile.defaultColor;
        middleCenterTile.Color = level.middleCenterTile.defaultColor;
        middleRightTile.Color = level.middleRightTile.defaultColor;
        bottomLeftTile.Color = level.bottomLeftTile.defaultColor;
        bottomMiddleTile.Color = level.bottomMiddleTile.defaultColor;
        bottomRightTile.Color = level.bottomRightTile.defaultColor;
    }

    private void SetTiles()
    {
        allTiles.Add(topLeftTile);
        allTiles.Add(topMiddleTile);
        allTiles.Add(topRightTile);
        allTiles.Add(middleLeftTile);
        allTiles.Add(middleCenterTile);
        allTiles.Add(middleRightTile);
        allTiles.Add(bottomLeftTile);
        allTiles.Add(bottomMiddleTile);
        allTiles.Add(bottomRightTile);
    }


    //private void HandlePatternCompletion()
    //{
    //    tilesToRemove.Clear();
    //    for (int i = 0; i < GetNumberOfTiles(); i++)
    //    {
    //        if (tiles[i].Color == ColorManager.Colors.None)
    //            continue;

    //        if (level.CurrentLevel.pattern[i] == tiles[i].Color)
    //        {
    //            tiles[i].ResetLevel();
    //            tiles[i].GetComponent<Animator>().SetTrigger("Flip");
    //        }
    //        else
    //        {
    //            tiles[i].GetComponent<Animator>().SetTrigger("ShrinkAway");
    //            AddTileToRemove(i);
    //        }
    //    }
    //    float animationDuration = 1f;
    //    StartCoroutine(CompleteLevel(animationDuration));
    //}
}
