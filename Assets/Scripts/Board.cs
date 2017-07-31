using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	public static Board instance = null;
	public List<Tile> tiles = new List<Tile> ();
	public int boardSize = 3;

	public delegate void OnRemoveTriplesEndAction ();

	public static event OnRemoveTriplesEndAction OnRemoveTriplesEnd;

	private Score score;
	private List<int> tilesToRefresh = new List<int> ();

	void Awake ()
	{
		ImplementSingleton ();
		FetchReferences ();
	}

	void Start ()
	{
		InitBoard ();
		SubscribeEvents ();
	}

	public void HideHints ()
	{
		foreach (Tile tile in tiles) {
			tile.HideHints ();
		}
	}

	private void FetchReferences ()
	{
		score = GameObject.FindGameObjectWithTag (Tags.Score).GetComponent<Score> ();
	}

	private void ImplementSingleton ()
	{
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	private void InitBoard ()
	{
		int numberOfTiles = boardSize * boardSize;
		for (int i = 0; i < numberOfTiles; i++) {
			tiles [i].Type = Tile.TileType.None;
		}
		if (numberOfTiles % 2 == 1) {
			tiles [numberOfTiles / 2].Type = Tile.TileType.Gray;
		}
	}

	private void RemoveTile (int index)
	{
		tiles [index].Type = Tile.TileType.None;
		tiles [index].GetComponent<Animator> ().SetTrigger ("ShrinkAway");
		if (!tilesToRefresh.Contains (index)) {
			tilesToRefresh.Add (index);
		}
	}

	private void RemoveTripleIfIdentical (int first, int second, int third)
	{
		if (
			tiles [first].Type == tiles [second].Type
			&& tiles [first].Type == tiles [third].Type
			&& tiles [first].Type != Tile.TileType.None
			&& tiles [first].Type != Tile.TileType.Gray) {
			int totalTileLevel = 0;
			RemoveTile (first);
			RemoveTile (second);
			RemoveTile (third);
			score.addTriple (totalTileLevel);
		}
	}

	IEnumerator OnRemoveTriplesAnimationFinished (float time)
	{
		yield return new WaitForSeconds (time);
		foreach (int index in tilesToRefresh) {
			tiles [index].GetComponent<Animator> ().SetTrigger ("Idle");
			tiles [index].ResetLevel ();
			tiles [index].Refresh ();
		}
		tilesToRefresh.Clear ();
		OnRemoveTriplesEnd ();
	}

	private void RemoveTriples ()
	{
		RemoveTripleIfIdentical (0, 1, 2);
		RemoveTripleIfIdentical (3, 4, 5);
		RemoveTripleIfIdentical (6, 7, 8);
		RemoveTripleIfIdentical (0, 3, 6);
		RemoveTripleIfIdentical (1, 4, 7);
		RemoveTripleIfIdentical (2, 5, 8);
		RemoveTripleIfIdentical (0, 4, 8);
		RemoveTripleIfIdentical (2, 4, 6);

		if (tilesToRefresh.Count > 0) {
			float animationDuration = 1f / tiles [0].GetComponent<Animator> ().speed;
			StartCoroutine (OnRemoveTriplesAnimationFinished (animationDuration));
		} else {
			OnRemoveTriplesEnd ();
		}
	}

	private void SubscribeEvents ()
	{
		Launcher.OnLaunchEnd += HideHints;
		Launcher.OnLaunchEnd += RemoveTriples;
	}
}
