using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	public Text pointsText;

	private int points = 0;
	public int pointsPerTurn = 10;
	public int pointsPerPop = 3;
	public int pointsPerMerge = 9;
	public int pointsPerTriple = 27;

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

	public void addPop ()
	{
		points += pointsPerPop;
	}

	public void addTriple ()
	{
		points += pointsPerTriple;
	}

	public void addTurn ()
	{
		points += pointsPerTurn;
	}
}
