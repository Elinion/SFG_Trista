﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTester : MonoBehaviour {

	public int worldId;
	public int levelGroupId;
	public int levelId;

	private void Awake() {
		SceneManager.LoadScene(Tags.Persistent, LoadSceneMode.Additive);
	}

	private void Start() {
		LevelData levelData = DataController.instance
			.getWorldById(worldId)
			.levelGroups[levelGroupId]
			.levels[levelId];
		GameController.instance.playLevel(levelData);
	}
}
