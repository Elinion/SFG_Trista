using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelColors : MonoBehaviour
{
	public List<Tile.TileType> colors = new List<Tile.TileType> ();

	public Tile.TileType getRandomColor ()
	{
		return colors [Random.Range (0, colors.Count)];
	}
}
