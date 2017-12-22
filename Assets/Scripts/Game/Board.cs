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
            get
            {
                if (allTiles.Count == 0)
                {
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

    public List<Line> lines = new List<Line>();
    public List<Tile> tiles = new List<Tile>();

    private int boardSize;

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
        tiles.ForEach(tile => tile.HideHints());
    }

    public void InitBoard(LevelData level)
    {
        if (tiles.Count != level.tiles.Length)
        {
            Debug.Log("Board::InitBoard: board size and level size don't match.");
            return;
        }
        boardSize = (int)Mathf.Sqrt((float)tiles.Count);

        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].Color = level.tiles[i].defaultColor;
        }
    }

    public bool IsLevelCompleted(LevelData level)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if (!DoesTileMatch(tiles[i], level.tiles[i]))
            {
                return false;
            }
        }

        return true;
    }

    public void PlayLevelCompletedAnimation(LevelData level)
    {
        for (int i = 0; i < tiles.Count; i++) {
            Tile currentTile = tiles[i];
            if(level.tiles[i].isRequired
               && DoesTileMatch(currentTile, level.tiles[i])) {
                currentTile.PlaySuccessAnimation();
            } else {
                currentTile.Remove();
            }
        }
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
}
