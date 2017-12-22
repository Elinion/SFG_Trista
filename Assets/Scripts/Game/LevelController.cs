using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    public TubeController tubeController;
    public PlayerControls playerControls;
    public Board board;
    public GameObject levelCompletedUI;

    private List<Tile> tilesBeingRemoved;
    private Tube lastTubePlayed;
    private int colorSequenceIndex = 0;
    private ColorManager.Colors[] defaultColors = new ColorManager.Colors[10];

    public void OnShiftEnd()
    {
        StartTurn();
    }

    public void GoToLevelMenu()
    {
        GameController.instance.GoToLevelMenu();
    }

    public void GoToNextLevel() {
        GameController.instance.GoToNextLevel();
    }

    public void OnTubePlayed()
    {
        CheckLevelProgress();
        RemoveAlignedTiles();
    }

    public void PlayTube(Tube tube)
    {
        if (tube.CanPlay())
        {
            playerControls.enabled = false;
            tube.Play();
            lastTubePlayed = tube;
        }
    }

    public void Restart()
    {
        GameController.instance.ReloadScene();
    }

    public void TileHasBeenRemoved(Tile tile)
    {
        if (tilesBeingRemoved != null
            && tilesBeingRemoved.Contains(tile))
        {
            tilesBeingRemoved.Remove(tile);
            if (tilesBeingRemoved.Count == 0)
            {
                CheckLevelProgress();
            }
        }

    }

    public void WaitForTilesRemoval(List<Tile> tilesBeingRemoved)
    {
        this.tilesBeingRemoved = tilesBeingRemoved;
    }

    private void Awake()
    {
        SetDefaultColors();
    }

    private void Start()
    {
        tubeController.tubes.ForEach(tube => tube.Color = GetRandomLevelColor());
        board.InitBoard(GameController.instance.Level);
        StartTurn();
    }

    private bool CanPlayAtLeastOneTube()
    {
        foreach (Tube tube in tubeController.tubes)
        {
            if (tube.CanPlay())
            {
                return true;
            }
        }
        return false;
    }

    private void CheckLevelProgress()
    {
        if (board.IsLevelCompleted(GameController.instance.Level))
        {
            GameCompleted();
        }
        else
        {
            board.HideTileHints();
            ChangeTubeColor();
            tubeController.ShiftTubes();
        }
    }

    private void ChangeTubeColor()
    {
        colorSequenceIndex = ++colorSequenceIndex % GameController.instance.Level.tubeSequence.Length;
        ColorGeneratorData colorGen = GameController.instance.Level.tubeSequence[colorSequenceIndex];
        ColorManager.Colors nextColor;
        if (colorGen.generateRandomColor)
        {
            nextColor = GetRandomLevelColor();
        }
        else
        {
            nextColor = colorGen.fixedColor;
        }
        lastTubePlayed.Color = nextColor;
    }

    private void GameCompleted()
    {
        board.HideTileHints();
        board.PlayLevelCompletedAnimation(GameController.instance.Level);
        StartCoroutine(ShowLevelCompletedUI(1f));
    }

    private IEnumerator ShowLevelCompletedUI(float waitForSeconds) {
        yield return new WaitForSeconds(waitForSeconds);
        levelCompletedUI.SetActive(true);
    }

    private void GameOver()
    {
        Debug.Log("Game over");
    }

    private ColorManager.Colors GetRandomLevelColor()
    {
        LevelData level = GameController.instance.Level;
        return level.possibleColors.Length == 0 ?
                    defaultColors[Random.Range(0, defaultColors.Length)] :
                    level.possibleColors[Random.Range(0, level.possibleColors.Length)];
    }

    private bool RemoveAlignedTiles()
    {
        List<Tile> alignedTiles = board.GetAlignedTiles();
        if (alignedTiles.Count > 0)
        {
            WaitForTilesRemoval(alignedTiles);
            alignedTiles.ForEach(tile =>
            {
                tile.Remove();
            });
            return true;
        }
        return false;
    }

    private void SetDefaultColors()
    {
        defaultColors[0] = ColorManager.Colors.Black;
        defaultColors[1] = ColorManager.Colors.Blue;
        defaultColors[2] = ColorManager.Colors.Gray;
        defaultColors[3] = ColorManager.Colors.Green;
        defaultColors[4] = ColorManager.Colors.Multicolor;
        defaultColors[5] = ColorManager.Colors.Orange;
        defaultColors[6] = ColorManager.Colors.Purple;
        defaultColors[7] = ColorManager.Colors.Red;
        defaultColors[8] = ColorManager.Colors.White;
        defaultColors[9] = ColorManager.Colors.Yellow;
    }

    private void ShowHints()
    {
        tubeController.tubes.ForEach(tube => tube.ShowHints());
    }

    private void StartTurn()
    {
        if (CanPlayAtLeastOneTube())
        {
            ShowHints();
            WaitForPlayerInput();
        }
        else
        {
            GameOver();
        }
    }

    private void WaitForPlayerInput()
    {
        playerControls.enabled = true;
    }

}
