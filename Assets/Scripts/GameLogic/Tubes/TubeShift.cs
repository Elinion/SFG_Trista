using UnityEngine;

public class TubeShift : MonoBehaviour {
    public Tube tube;
    
    private Turn turn;

    private static int remainingShifts = 12;

    public void OnShiftEnd() {
        tube.takeNextColor();
        remainingShifts--;
        if (remainingShifts != 0) {
            return;
        }

        remainingShifts = 12;
        turn.onTubeShiftEnd();
    }

    private void Awake() {
        turn = GameObject.FindGameObjectWithTag(Tags.LevelController).GetComponent<Turn>();
    }
}