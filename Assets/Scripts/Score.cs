using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	public Text pointsText;

	private int points = 0;
	public int launchPoints;
	public int growPoints;
	public int mergePoints;
	public int popPoints;
	public int extraPointsPerLevel = 10;

	public int Points {
		get { return points; }
	}

	void Update ()
	{
		pointsText.text = points.ToString ();
	}

	public void addMergePoints ()
	{
		points += mergePoints;
	}

	public void addGrowPoints ()
	{
		points += growPoints;
	}

	public void addTriple (int totalLevel = 0)
	{
		points += popPoints + totalLevel * extraPointsPerLevel;
	}

	public void addLaunchPoints ()
	{
		points += launchPoints;
	}
}
