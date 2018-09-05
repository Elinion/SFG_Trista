using System.Collections.Generic;
using GameData;
using UnityEngine;
using UnityEngine.UI;

public class LevelGroupSelectionMenu : MonoBehaviour {

	public Text worldName;
	public LevelGroupIcon[] levelGroupIcons = new LevelGroupIcon[3];

	void Awake() {
		hideLevelGroupIcons();
	}

	void Start () {
		setUpUI();
	}

	public void goBackToMainMenu() {
		GameController.instance.goToMainMenuAndSkipAnimation();
	}

	private void setUpUI() {
		World world = GameController.instance.getWorld();
		worldName.text = world.name;
		setUpLevelGroupIcons(world.levelGroups);
	}

	private void hideLevelGroupIcons() {
		foreach (LevelGroupIcon levelGroupIcon in levelGroupIcons) {
			levelGroupIcon.gameObject.SetActive(false);
		}
	}

	private void setUpLevelGroupIcons(IList<LevelGroup> levelGroups) {
		if (levelGroups.Count > levelGroupIcons.Length) {
			Debug.Log("There are more level groups than level group icons. Update level group selection menu to allow more level groups.");	
		}
		
		for (int i = 0; i < levelGroups.Count; i++) {
			levelGroupIcons[i].gameObject.SetActive(true);
			levelGroupIcons[i].setLevelGroup(levelGroups[i]);
		}
	}
}
