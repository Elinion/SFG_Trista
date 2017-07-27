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
	public Sprite rainbomb;
	public ParticleSystem mergeHint;

	private Tile.TileType type;
	private Score score;

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
			case Tile.TileType.Rainbomb:
				sprite = rainbomb;
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
		score = GameObject.FindGameObjectWithTag (Tags.Score).GetComponent<Score> ();
		mergeHint.Pause ();
		ChangeType ();
	}

	public void ChangeType ()
	{
		Tile.TileType newType = (Tile.TileType)Random.Range (0, 9);
		Type = newType;
	}

	public void ShowHints ()
	{
		if (Type == Tile.TileType.Rainbomb) {
			mergeHint.Stop ();
			return;
		}
		for (int i = 0; i < targetTilesIndexes.Count; i++) {
			int tileIndex = targetTilesIndexes [i];
			Tile tile = TileManager.instance.tiles [tileIndex];
			if (tile.Type == Tile.TileType.None) {
				continue;
			}
			if (tile.CanMerge (Type)) {
				mergeHint.Play ();
			} else {
				mergeHint.Stop ();
			}
			break;
		}
	}

	public void Trigger ()
	{
		bool didLaunch = LaunchOnFarthestTile ();
		if (didLaunch) {
			score.addTurn ();
			ChangeType ();
		} else {
			GameController.instance.GameOver ();
		}
	}

	private bool LaunchOnFarthestTile ()
	{
		for (int i = 0; i < targetTilesIndexes.Count; i++) {
			int targetTileIndex = targetTilesIndexes [i];
			Tile targetTile = TileManager.instance.tiles [targetTileIndex];
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
					Tile previousTile = TileManager.instance.tiles [targetTilesIndexes [i - 1]];
					LaunchOnTile (previousTile);
					return true;
				}
			}
		}
		return false;
	}

	private bool LaunchOnTile (Tile tile)
	{
		if (Type == Tile.TileType.Rainbomb || Type == tile.Type) {
			PopTile (tile);
			return true;
		}
		if (tile.Type == Tile.TileType.None) {
			tile.Type = Type;
			return true;
		} 
		if (tile.CanMerge (Type)) {
			tile.Merge (Type);
			score.addMerge ();
			return true;
		}
		return false;
	}

	private void PopTile (Tile tile)
	{
		tile.Pop ();
		score.addPop ();
	}
}
