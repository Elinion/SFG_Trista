using GameData;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelController : MonoBehaviour {
    public TubeController tubeController;
    public Board board;
    public LevelEnd levelEnd;
    public Turn turn;


    private void Start() {
        setUpTubeColors();
        setUpBoardTiles();
        turn.start();
    }

    private void setUpTubeColors() {
        Colors[] defaultTubeColors = GameController.instance.level.defaultTubeColors;
        if (defaultTubeColors.Length > 0) {
            setTubeColors(defaultTubeColors);
        } else {
            setRandomTubeColors();
        }
    }

    private void setTubeColors(Colors[] colors) {
        if (colors.Length != tubeController.tubes.Count) {
            Debug.Log("Failed to assign tube colors on level setup: " + GameController.instance.level.name +
                      ". The number of provided colors is not correct.");
            return;
        }

        for (int i = 0; i < tubeController.tubes.Count; i++) {
            tubeController.tubes[i].Color = colors[i];
        }
    }

    private void setRandomTubeColors() {
        tubeController.tubes.ForEach(tube => tube.Color = ColorManager.instance.getRandomLevelColor());
    }

    private void setUpBoardTiles() {
        board.initBoard(GameController.instance.level);
    }

    public void onTurnOver() {
        if (tubeController.hasPlayableTube()) {
            turn.start();
        } else {
            endLevel(GameProgress.State.NotOk);
        }
    }

    public void onTubePlayed() {
        GameProgress.State state = getLevelProgress();
        if (state == GameProgress.State.NotOk) {
            turn.end();
        } else {
            endLevel(state);
        }
    }

    private GameProgress.State getLevelProgress() {
        GameData.Tile[] expectedTiles = GameController.instance.level.tiles;
        bool hasDirtyTile = false;
        for (int i = 0; i < board.tiles.Count; i++) {
            GameData.Tile expectedTile = expectedTiles[i];
            Tile actualTile = board.tiles[i];

            if (expectedTile.required) {
                if (!Tile.isValid(expectedTile, actualTile)) {
                    return GameProgress.State.NotOk;
                }
            } else if (actualTile.Color != Colors.None) {
                hasDirtyTile = true;
            }
        }

        return hasDirtyTile ? GameProgress.State.Ok : GameProgress.State.Perfect;
    }

    private void endLevel(GameProgress.State state) {
        board.HideTileHints();

        if (state == GameProgress.State.Ok || state == GameProgress.State.Perfect) {
            board.playLevelCompleteAnimation(GameController.instance.level);
        }

        levelEnd.triggerEnd(state);
    }

    public void goToLevelMenu() {
        GameController.instance.goToLevelSelectionMenu();
    }

    public void restart() {
        GameController.instance.reloadScene();
    }
}