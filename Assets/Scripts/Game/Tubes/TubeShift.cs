using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeShift : MonoBehaviour {

    public Tube tube;
    LevelController levelController;

    private static int remainingShifts = 12;

    public void OnShiftEnd()
    {
        tube.TakeNextColor();
        remainingShifts--;
        if(remainingShifts == 0) {
            remainingShifts = 12;
            levelController.OnShiftEnd();
        }
    }

    private void Awake()
    {
        levelController = GameObject.FindGameObjectWithTag(Tags.LevelController).GetComponent<LevelController>();
    }
}
