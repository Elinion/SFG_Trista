using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {
    public TubeController tubeController;
    public PlayerControls playerControls;
    public Board board;
    public GameObject levelOkMessage;
    public GameObject levelPerfectMessage;

    private List<Tile> tilesBeingRemoved;
    private Tube lastTubePlayed;
    private int colorSequenceIndex = 0;
    private ColorManager.Colors[] defaultColors = new ColorManager.Colors[10];

    private enum LevelProgress {
        NotOk,
        Ok,
        Perfect
    }

    private void Awake() {
        setDefaultColors();
    }

    private void Start() {
        setUpTubeColors();
        setUpBoardTiles();
        startTurn();
    }

    private void setUpTubeColors() {
        tubeController.tubes.ForEach(tube => tube.Color = getRandomLevelColor());
    }

    private void setUpBoardTiles() {
        board.InitBoard(GameController.instance.Level);
    }

    private void startTurn() {
        if (tubeController.hasPlayableTube()) {
            tubeController.showTubeHints();
            waitForPlayerInput();
        } else {
            gameOver();
        }
    }

    public void onShiftEnd() {
        startTurn();
    }

    public void onTubePlayed() {
        LevelProgress levelProgress = getLevelProgress();
        if (levelProgress == LevelProgress.NotOk) {
            endTurn();
        } else {
            endLevel(levelProgress);
        }
    }

    private LevelProgress getLevelProgress() {
        TileData[] expectedTiles = GameController.instance.Level.tiles;
        bool hasDirtyTile = false;
        for (int i = 0; i < board.tiles.Count; i++) {
            TileData expectedTile = expectedTiles[i];
            Tile actualTile = board.tiles[i];

            if (expectedTile.isRequired) {
                if (!Tile.isValid(expectedTile, actualTile)) {
                    return LevelProgress.NotOk;
                }
            } else if (actualTile.Color != ColorManager.Colors.None) {
                hasDirtyTile = true;
            }
        }

        return hasDirtyTile ? LevelProgress.Ok : LevelProgress.Perfect;
    }

    private void endTurn() {
        board.HideTileHints();
        changeLastPlayedTubeColor();
        tubeController.shiftTubes();
        removeAlignedTiles();
    }

    private void endLevel(LevelProgress levelProgress) {
        board.HideTileHints();
        board.PlayLevelCompleteAnimation(GameController.instance.Level);
        showLevelEndMessage(levelProgress);
    }

    private void showLevelEndMessage(LevelProgress levelProgress) {
        const float waitForSeconds = 1f;
        switch (levelProgress) {
            case LevelProgress.Ok:
                StartCoroutine(showOkEndMessage(waitForSeconds));
                break;
            case LevelProgress.Perfect:
                StartCoroutine(showPerfectEndMessage(waitForSeconds));
                break;
        }
    }

    private IEnumerator showOkEndMessage(float waitForSeconds) {
        yield return new WaitForSeconds(waitForSeconds);
        levelOkMessage.SetActive(true);
    }

    private IEnumerator showPerfectEndMessage(float waitForSeconds) {
        yield return new WaitForSeconds(waitForSeconds);
        levelPerfectMessage.SetActive(true);
    }

    public void playTube(Tube tube) {
        if (!tube.CanPlay()) return;

        playerControls.enabled = false;
        tube.Play();
        lastTubePlayed = tube;
    }

    public void GoToLevelMenu() {
        GameController.instance.GoToLevelMenu();
    }

    public void GoToNextLevel() {
        GameController.instance.GoToNextLevel();
    }

    public void Restart() {
        GameController.instance.ReloadScene();
    }

    public void tileHasBeenRemoved(Tile tile) {
        if (tilesBeingRemoved == null || !tilesBeingRemoved.Contains(tile))
            return;

        tilesBeingRemoved.Remove(tile);
    }

    private void WaitForTilesRemoval(List<Tile> tilesBeingRemoved) {
        this.tilesBeingRemoved = tilesBeingRemoved;
    }


    private void changeLastPlayedTubeColor() {
        colorSequenceIndex = ++colorSequenceIndex % GameController.instance.Level.tubeSequence.Length;
        ColorGeneratorData colorGen = GameController.instance.Level.tubeSequence[colorSequenceIndex];
        lastTubePlayed.Color = colorGen.generateRandomColor ? getRandomLevelColor() : colorGen.fixedColor;
    }

    private void gameOver() {
        Debug.Log("Game over");
    }

    private ColorManager.Colors getRandomLevelColor() {
        LevelData level = GameController.instance.Level;
        return level.possibleColors.Length == 0
            ? defaultColors[Random.Range(0, defaultColors.Length)]
            : level.possibleColors[Random.Range(0, level.possibleColors.Length)];
    }

    private void removeAlignedTiles() {
        List<Tile> alignedTiles = board.GetAlignedTiles();
        if (alignedTiles.Count <= 0) return;

        WaitForTilesRemoval(alignedTiles);
        alignedTiles.ForEach(tile => { tile.Remove(); });
    }

    private void setDefaultColors() {
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

    private void waitForPlayerInput() {
        playerControls.enabled = true;
    }
}