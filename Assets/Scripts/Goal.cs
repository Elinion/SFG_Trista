using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
	public List<Image> targetTilesUI = new List<Image> ();
	public Sprite black;
	public Sprite blue;
	public Sprite gray;
	public Sprite green;
	public Sprite orange;
	public Sprite purple;
	public Sprite red;
	public Sprite white;
	public Sprite yellow;
    public Sprite transparent;

	void Start ()
	{
		LevelColors.LevelParams level = GetComponent<LevelColors> ().Level;
		for (int i = 0; i < 9; i++) {
			targetTilesUI [i].sprite = TypeToSprite (level.pattern [i]);
		}
	}

	private Sprite TypeToSprite (Tile.TileType type)
	{
		switch (type) {
		case Tile.TileType.Black:
			return black;
		case Tile.TileType.Blue:
			return blue;
		case Tile.TileType.Green:
			return green;
		case Tile.TileType.Orange:
			return orange;
		case Tile.TileType.Purple:
			return purple;
		case Tile.TileType.Red:
			return red;
		case Tile.TileType.White:
			return white;
		case Tile.TileType.Yellow:
			return yellow;
		case Tile.TileType.Gray:
			return gray;
		}
        return transparent;
	}
}
