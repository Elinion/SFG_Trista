using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
	public int position;
	public List<int> targetTilesIndexes = new List<int> ();
	public Sprite black;
	public Sprite blue;
	public Sprite green;
	public Sprite orange;
	public Sprite purple;
	public Sprite red;
	public Sprite white;
	public Sprite yellow;

	private TileManager tileManager;
	private Tile.TileType type;

	public Tile.TileType Type {
		get { return type; }
		set {
			type = value;
			Sprite sprite;
			switch (value) {
			case Tile.TileType.Black:
				sprite = black;
				break;
			case Tile.TileType.Blue:
				sprite = blue;
				break;
			case Tile.TileType.Green:
				sprite = green;
				break;
			case Tile.TileType.Orange:
				sprite = orange;
				break;
			case Tile.TileType.Purple:
				sprite = purple;
				break;
			case Tile.TileType.Red:
				sprite = red;
				break;
			case Tile.TileType.White:
				sprite = white;
				break;
			case Tile.TileType.Yellow:
				sprite = yellow;
				break;
			default:
				sprite = null;
				break;
			}
			GetComponent<SpriteRenderer> ().sprite = sprite;
		}
	}

	void Awake ()
	{
		tileManager = GameObject.FindGameObjectWithTag (Tags.TileManager).GetComponent<TileManager> ();
	}

	void Start ()
	{
		ChangeType ();
	}

	public void ChangeType ()
	{
		Tile.TileType newType = (Tile.TileType)Random.Range (0, 8);
		Type = newType;
	}

	public void Trigger ()
	{
		bool didLaunch = LaunchOnFarthestTile ();
		if (didLaunch) {
			ChangeType ();
		} else {
			GameController.instance.GameOver ();
		}
	}

	private bool LaunchOnFarthestTile ()
	{
		for (int i = 0; i < targetTilesIndexes.Count; i++) {
			int targetTileIndex = targetTilesIndexes [i];
			Tile targetTile = tileManager.tiles [targetTileIndex];
			if (targetTile.Type == Tile.TileType.None && i < targetTilesIndexes.Count - 1) {
				continue;
			}
			bool launchSuccessful = LaunchOnTile (targetTile);
			if (launchSuccessful) {
				return true;
			} else {
				if (i == 0) {
					return false;
				} else {
					Tile previousTile = tileManager.tiles [targetTilesIndexes [i - 1]];
					LaunchOnTile (previousTile);
					return true;
				}
			}
		}
		return false;
	}

	private bool LaunchOnTile (Tile tile)
	{
		bool mergeSuccessful = tile.Merge (type);
		if (!mergeSuccessful) {
			if (tile.Type == type) {
				tile.Pop ();
			} else {
				return false;
			}
		}
		return true;
	}
}
