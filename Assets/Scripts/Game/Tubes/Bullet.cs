using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Tube tube;

    private LevelController levelController;

    public void MergeWithTarget(int distance) {
        switch(distance) {
            case 1:
                tube.firstTarget.AddColor(tube.Color);
                break;
            case 2:
                tube.secondTarget.AddColor(tube.Color);
                break;
            case 3:
                tube.thirdTarget.AddColor(tube.Color);
                break;
        }
        levelController.OnTubePlayed();
    }

    private void Awake()
    {
        levelController = GameObject.FindGameObjectWithTag(Tags.LevelController).GetComponent<LevelController>();
    }
}
