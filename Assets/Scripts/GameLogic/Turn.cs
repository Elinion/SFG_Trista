using System.Collections.Generic;
using GameData;
using UnityEngine;

public class Turn : MonoBehaviour {
    public TubeController tubeController;
    public PlayerControls playerControls;
    public LevelController levelController;
    public Board board;

    private int colorSequenceIndex;
    private Tube tubeToRefresh;

    public void start() {
        tubeController.showTubeHints();
        waitForPlayerInput();
    }

    public void end() {
        board.HideTileHints();
        removeAlignedTiles();
        shiftTubes();
    }

    public void playTube(Tube tube) {
        if (!tube.canPlay()) {
            return;
        }

        playerControls.enabled = false;
        tube.play();
        tubeToRefresh = tubeController.getNextTube(tube);
    }

    public void onTubeShiftEnd() {
        if (shouldChangeLastPlayedTubeColor()) {
            changeLastPlayedTubeColor();
        }

        levelController.onTurnOver();
    }

    private void waitForPlayerInput() {
        playerControls.enabled = true;
    }

    private void shiftTubes() {
        if (GameController.instance.level.rotateAfterPlay) {
            tubeController.shiftTubes();
        } else {
            onTubeShiftEnd();
        }
    }

    private void changeLastPlayedTubeColor() {
        colorSequenceIndex = ++colorSequenceIndex % getTubeSequenceLength();
        Colors nextColor = GameController.instance.level.tubeColors[colorSequenceIndex];
        tubeToRefresh.Color = nextColor;
    }

    private static bool shouldChangeLastPlayedTubeColor() {
        return getTubeSequenceLength() > 0;
    }

    private static int getTubeSequenceLength() {
        return GameController.instance.level.tubeColors.Length;
    }

    private void removeAlignedTiles() {
        List<Tile> alignedTiles = board.getAlignedTiles();
        if (alignedTiles.Count <= 0) {
            return;
        }

        alignedTiles.ForEach(tile => { tile.remove(); });
    }
}