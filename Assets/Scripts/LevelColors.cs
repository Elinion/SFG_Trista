using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelColors : MonoBehaviour
{
	[System.Serializable]
	public class LevelParams
	{
		public int levelId;
		public string levelName = "";
		public List<Tile.TileType> pattern = new List<Tile.TileType> ();
		public List<Tile.TileType> colors = new List<Tile.TileType> ();
	}

	public List<LevelParams> levels = new List<LevelParams> ();

	private LevelParams currentLevel;

	public LevelParams CurrentLevel {
		get { return currentLevel; }
	}

	public LevelParams Level {
		get { return currentLevel; }
	}

	void Awake ()
	{
		loadCurrentLevelParams ();
	}

	public Tile.TileType getRandomColor ()
	{
		return currentLevel.colors [Random.Range (0, currentLevel.colors.Count)];
	}

	private void loadCurrentLevelParams ()
	{
		foreach (LevelParams level in levels) {
			if (level.levelId == GlobalLevelData.selectedLevelId) {
				currentLevel = level;
			}
		}
	}
}
