using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeController : MonoBehaviour
{
    public List<Tube> tubes = new List<Tube> ();
	public float launcherAnimatorSpeed;

    public void ShiftTubes ()
	{
        for (int i = 0; i < tubes.Count; i++) {
            int previousTube = i == 0 ? tubes.Count - 1 : i - 1;
            tubes[i].Shift(tubes[previousTube].Color);
        }
    }

    private void Start()
    {
        tubes.ForEach(tube => tube.tubeAnimator.speed = launcherAnimatorSpeed);
    }
}
