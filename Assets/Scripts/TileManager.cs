using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
	public static TileManager instance = null;
	public List<Tile> tiles = new List<Tile> ();

	private Score score;

	void Awake ()
	{
		ImplementSingleton ();
		score = GameObject.FindGameObjectWithTag (Tags.Score).GetComponent<Score> ();
	}

	public void RemoveTriples ()
	{
		RemoveTripleIfIdentical (0, 1, 2);
		RemoveTripleIfIdentical (3, 4, 5);
		RemoveTripleIfIdentical (6, 7, 8);
		RemoveTripleIfIdentical (0, 3, 6);
		RemoveTripleIfIdentical (1, 4, 7);
		RemoveTripleIfIdentical (2, 5, 8);
		RemoveTripleIfIdentical (0, 4, 8);
		RemoveTripleIfIdentical (2, 4, 6);
	}

	private void RemoveTripleIfIdentical (int first, int second, int third)
	{
		if (tiles [first].Type == Tile.TileType.None
		    || tiles [first].Type == Tile.TileType.Gray
		    || tiles [second].Type == Tile.TileType.Gray
		    || tiles [second].Type == Tile.TileType.Gray
		    || tiles [third].Type == Tile.TileType.Gray
		    || tiles [third].Type == Tile.TileType.Gray) {
			return;
		}
		if (tiles [first].Type == tiles [second].Type && tiles [first].Type == tiles [third].Type) {
			int totalTileLevel = 0;

			tiles [first].Type = Tile.TileType.None;
			totalTileLevel += tiles [first].Level;
			tiles [first].ResetLevel ();

			tiles [second].Type = Tile.TileType.None;
			totalTileLevel += tiles [second].Level;
			tiles [second].ResetLevel ();

			tiles [third].Type = Tile.TileType.None;
			totalTileLevel += tiles [third].Level;
			tiles [third].ResetLevel ();

			score.addTriple (totalTileLevel);
		}
	}

	private void ImplementSingleton ()
	{
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}
}
