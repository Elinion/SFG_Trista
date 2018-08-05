using UnityEngine;

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
        ColorManager.Colors[] startingTubeColors = GameController.instance.levelData.startingTubeColors;
        if (startingTubeColors.Length > 0) {
            setTubeColors(startingTubeColors);
        } else {
            setRandomTubeColors();
        }
    }

    private void setTubeColors(ColorManager.Colors[] colors) {
        if (colors.Length != tubeController.tubes.Count) {
            Debug.Log("Failed to assign tube colors on level setup: " + GameController.instance.levelData.levelName +
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
        board.initBoard(GameController.instance.levelData);
    }

    public void onTurnOver() {
        if (tubeController.hasPlayableTube()) {
            turn.start();
        } else {
            endLevel(Level.Progress.NotOk);
        }
    }

    public void onTubePlayed() {
        Level.Progress levelProgress = getLevelProgress();
        if (levelProgress == Level.Progress.NotOk) {
            turn.end();
        } else {
            endLevel(levelProgress);
        }
    }

    private Level.Progress getLevelProgress() {
        TileData[] expectedTiles = GameController.instance.levelData.tiles;
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

        if (levelProgress == Level.Progress.Ok || levelProgress == Level.Progress.Perfect) {
            board.playLevelCompleteAnimation(GameController.instance.levelData);
        }

        levelEnd.showEnd(levelProgress);
    }

    public void goToLevelMenu() {
        GameController.instance.GoToLevelMenu();
    }

    public void restart() {
        GameController.instance.reloadScene();
    }
}