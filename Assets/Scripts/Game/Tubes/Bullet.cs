using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Tube tube;

    private LevelController levelController;

    public void MergeWithTarget(int distance) {
        tube.targetTiles[distance].AddColor(tube.Color);
        levelController.onTubePlayed();
    }

    private void Awake()
    {
        levelController = GameObject.FindGameObjectWithTag(Tags.LevelController).GetComponent<LevelController>();
    }
}
