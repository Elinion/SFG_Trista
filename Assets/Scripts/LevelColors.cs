using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelColors : MonoBehaviour
{
	[System.Serializable]
	public class LevelParams
	{
		public int levelId;
		public string levelName = "";
        public List<ColorManager.Colors> pattern = new List<ColorManager.Colors> ();
        public List<ColorManager.Colors> colors = new List<ColorManager.Colors> ();
	}

	public List<LevelParams> levels = new List<LevelParams> ();
	public Text levelNameUI;

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
		levelNameUI.text = currentLevel.levelName;
	}

	private void loadCurrentLevelParams ()
	{
		// Survival mode
		if (GlobalLevelData.selectedLevelId == -1) {
			currentLevel = levels [0];
		} 
		// Levels with a pattern to match
		else {
			foreach (LevelParams level in levels) {
				if (level.levelId == GlobalLevelData.selectedLevelId) {
					currentLevel = level;
				}
			}
		}
	}
}
