using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public List<Tile> tiles = new List<Tile>();
    public int boardSize = 3;
    public bool survivalMode = false;

    public delegate void OnRemoveTriplesEndAction();

    public event OnRemoveTriplesEndAction OnRemoveTriplesEnd;

    private Score score;
    private List<int> tilesToRefresh = new List<int>();
    private LevelColors level;
    private GameObject[] launchers;
    private List<int> tilesToRemove = new List<int>();

    void Awake()
    {
        score = GameObject.FindGameObjectWithTag(Tags.Score).GetComponent<Score>();
        launchers = GameObject.FindGameObjectsWithTag(Tags.Launcher);
    }

    void Start()
    {
        InitBoard();
        level = GameObject.FindGameObjectWithTag(Tags.LevelController).GetComponent<LevelColors>();
        SubscribeEvents();
    }

    public void HideHints()
    {
        foreach (Tile tile in tiles)
        {
            tile.HideHints();
        }
    }

    private void AddTileToRemove(int index)
    {
        if (!tilesToRemove.Contains(index))
        {
            tilesToRemove.Add(index);
        }
    }

    private void CheckBoardState() {
        if (!survivalMode && IsLevelComplete()) { 
            GameController.instance.ClearLevel();
        } else {
            RemoveAlignedTiles();
            CheckGameOver();
        }
    }

    private bool IsLevelComplete()
    {
        for (int i = 0; i < 9; i++)
        {
            if (level.CurrentLevel.pattern[i] != Tile.TileType.None
                && level.CurrentLevel.pattern[i] != tiles[i].Type)
            {
                return false;
            }
        }
        return true;
    }

    private void CheckGameOver()
    {
        if (!LauncherManager.instance.CanUseAtLeastOnLauncher())
        {
            GameController.instance.GameOver();
        }
	}

	private void InitBoard()
	{
		int numberOfTiles = boardSize * boardSize;
		for (int i = 0; i < numberOfTiles; i++)
		{
			tiles[i].Type = Tile.TileType.None;
		}
		if (numberOfTiles % 2 == 1)
		{
			tiles[numberOfTiles / 2].Type = Tile.TileType.Gray;
		}
	}

    private void MarkTilesThatShouldBeRemoved()
    {
        tilesToRemove.Clear();
        RemoveTripleIfIdentical(0, 1, 2);
        RemoveTripleIfIdentical(3, 4, 5);
        RemoveTripleIfIdentical(6, 7, 8);
        RemoveTripleIfIdentical(0, 3, 6);
        RemoveTripleIfIdentical(1, 4, 7);
        RemoveTripleIfIdentical(2, 5, 8);
        RemoveTripleIfIdentical(0, 4, 8);
        RemoveTripleIfIdentical(2, 4, 6);
    }

    private void RemoveTile(int index)
    {
        tiles[index].Type = Tile.TileType.None;
        tiles[index].GetComponent<Animator>().SetTrigger("ShrinkAway");
        if (!tilesToRefresh.Contains(index))
        {
            tilesToRefresh.Add(index);
        }
    }


    private void RemoveTripleIfIdentical(int first, int second, int third)
    {
        if (
            tiles[first].Type == tiles[second].Type
            && tiles[first].Type == tiles[third].Type
            && tiles[first].Type != Tile.TileType.None
            && tiles[first].Type != Tile.TileType.Gray)
        {
            int totalTileLevel = 0;
            AddTileToRemove(first);
            AddTileToRemove(second);
            AddTileToRemove(third);
            score.addTriple(totalTileLevel);
        }
    }

    IEnumerator OnRemoveTriplesAnimationFinished(float time)
    {
		yield return new WaitForSeconds(time);
        CheckGameOver();
		ResetRemovedTiles();
		OnRemoveTriplesEnd();
    }

    private void RemoveAlignedTiles()
    {
        MarkTilesThatShouldBeRemoved();
        RemoveMarkedTiles();
        WaitForRemoveAnimationsAndResetRemovedTiles();
    }

    private void RemoveMarkedTiles()
    {
        foreach (int index in tilesToRemove)
        {
            RemoveTile(index);
        }
    }

    private void ResetRemovedTiles() {
		foreach (int index in tilesToRefresh)
		{
			tiles[index].GetComponent<Animator>().SetTrigger("Idle");
			tiles[index].ResetLevel();
			tiles[index].Refresh();
		}
		tilesToRefresh.Clear();
    }

    private void SubscribeEvents()
    {
        foreach (GameObject launcherGO in launchers)
        {
            Launcher launcher = launcherGO.GetComponent<Launcher>();
            launcher.OnLaunchEnd += HideHints;
            launcher.OnLaunchEnd += CheckBoardState;
        }
    }

    private void WaitForRemoveAnimationsAndResetRemovedTiles()
    {
        if (tilesToRefresh.Count > 0)
        {
            float animationDuration = 1f / tiles[0].GetComponent<Animator>().speed;
            StartCoroutine(OnRemoveTriplesAnimationFinished(animationDuration));
        }
        else
        {
            OnRemoveTriplesEnd();
        }
    }
}
