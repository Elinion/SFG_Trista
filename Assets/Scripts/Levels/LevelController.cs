using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {
    public TubeController tubeController;
    public PlayerControls playerControls;
    public Board board;
    public LevelEnd levelEnd;

    private List<Tile> tilesBeingRemoved;
    private Tube lastTubePlayed;
    private int colorSequenceIndex = 0;

    private void Start() {
        setUpTubeColors();
        setUpBoardTiles();
        startTurn();
    }

    private void setUpTubeColors() {
        tubeController.tubes.ForEach(tube => tube.Color = getRandomLevelColor());
    }

    private void setUpBoardTiles() {
        board.InitBoard(GameController.instance.level);
    }

    private void startTurn() {
        if (tubeController.hasPlayableTube()) {
            tubeController.showTubeHints();
            waitForPlayerInput();
        } else {
            endLevel();
        }
    }

    private void endTurn() {
        board.HideTileHints();
        changeLastPlayedTubeColor();
        tubeController.shiftTubes();
        removeAlignedTiles();
    }

    public void onTubeShiftEnd() {
        startTurn();
    }

    public void onTubePlayed() {
        Level.Progress levelProgress = getLevelProgress();
        if (levelProgress == Level.Progress.NotOk) {
            endTurn();
        } else {
            endLevel(levelProgress);
        }
    }

    private Level.Progress getLevelProgress() {
        TileData[] expectedTiles = GameController.instance.level.tiles;
        bool hasDirtyTile = false;
        for (int i = 0; i < board.tiles.Count; i++) {
            TileData expectedTile = expectedTiles[i];
            Tile actualTile = board.tiles[i];

            if (expectedTile.isRequired) {
                if (!Tile.isValid(expectedTile, actualTile)) {
                    return Level.Progress.NotOk;
                }
            } else if (actualTile.Color != ColorManager.Colors.None) {
                hasDirtyTile = true;
            }
        }

        return hasDirtyTile ? Level.Progress.Ok : Level.Progress.Perfect;
    }

    private void endLevel(Level.Progress levelProgress) {
        board.HideTileHints();
        board.PlayLevelCompleteAnimation(GameController.instance.level);
        levelEnd.showEnd(levelProgress);
    }

    public void playTube(Tube tube) {
        if (!tube.CanPlay()) return;

        playerControls.enabled = false;
        tube.Play();
        lastTubePlayed = tube;
    }

    public void goToLevelMenu() {
        GameController.instance.GoToLevelMenu();
    }

    public void goToNextLevel() {
        GameController.instance.GoToNextLevel();
    }

    public void restart() {
        GameController.instance.reloadScene();
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
        colorSequenceIndex = ++colorSequenceIndex % GameController.instance.level.tubeSequence.Length;
        ColorGeneratorData colorGen = GameController.instance.level.tubeSequence[colorSequenceIndex];
        lastTubePlayed.Color = colorGen.generateRandomColor ? getRandomLevelColor() : colorGen.fixedColor;
    }

    private void endLevel() {
        Debug.Log("Game over");
    }

    private ColorManager.Colors getRandomLevelColor() {
        LevelData level = GameController.instance.level;
        return level.possibleColors.Length == 0
            ? ColorManager.getRandomThrowableColor()
            : ColorManager.getRandomColorFromArray(level.possibleColors);
    }

    private void removeAlignedTiles() {
        List<Tile> alignedTiles = board.GetAlignedTiles();
        if (alignedTiles.Count <= 0) return;

        WaitForTilesRemoval(alignedTiles);
        alignedTiles.ForEach(tile => { tile.Remove(); });
    }

    private void waitForPlayerInput() {
        playerControls.enabled = true;
    }
}