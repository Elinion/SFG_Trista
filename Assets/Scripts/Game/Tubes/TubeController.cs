using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeController : MonoBehaviour {
    public List<Tube> tubes = new List<Tube>();
    public float launcherAnimatorSpeed;

    private void Start() {
        tubes.ForEach(tube => tube.tubeAnimator.speed = launcherAnimatorSpeed);
    }

    public void shiftTubes() {
        for (int i = 0; i < tubes.Count; i++) {
            int previousTube = i == 0 ? tubes.Count - 1 : i - 1;
            tubes[i].shift(tubes[previousTube].Color);
        }
    }

    public bool hasPlayableTube() {
        return tubes.Exists(tube => tube.canPlay());
    }

    public void showTubeHints() {
        tubes.ForEach(tube => tube.showHints());
    }

    public Tube getNextTube(Tube tube) {
        for (int i = 0; i < tubes.Count; i++) {
            if (tubes[i] == tube) {
                int nextTube = (i + 1) % tubes.Count;
                return tubes[nextTube];
            }
        }

        return null;
    }
}