using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	public Text pointsText;

	private int points = 0;
	public int pointsPerTurn = 10;
	public int pointsPerGrow = 3;
	public int pointsPerMerge = 9;
	public int pointsPerTriple = 27;
	public int extraPointsPerLevel = 10;

	public int Points {
		get { return points; }
	}

	void Update ()
	{
		pointsText.text = points.ToString ();
	}

	public void addMerge ()
	{
		points += pointsPerMerge;
	}

	public void addGrow ()
	{
		points += pointsPerGrow;
	}

	public void addTriple (int totalLevel = 0)
	{
		points += pointsPerTriple + totalLevel * extraPointsPerLevel;
	}

	public void addTurn ()
	{
		points += pointsPerTurn;
	}
}
