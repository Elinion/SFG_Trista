using System;
using GameData;
using GameProgress;
using UnityEngine;
using UnityEngine.UI;
using Game = GameData.Game;
using Level = GameData.Level;
using LevelGroup = GameData.LevelGroup;

public class LevelGroupProgress : MonoBehaviour {
    public Text earnedStarsUi;
    public Text maxStarsUi;
    public int worldIndex;
    public int levelGroupIndex;

    private int levelPerfectStarMultiplier = 3;

    private void Start() {
        if (worldIndex == -1) {
            worldIndex = GameController.instance.getWorld().index;
        }

        if (levelGroupIndex == -1) {
            levelGroupIndex = GameController.instance.getLevelGroup().index;
        }

        computeEarnedStars();
    }

    public void computeEarnedStars() {
        LevelGroup levelGroup = ByIndexAccessor.getLevelGroup(worldIndex, levelGroupIndex);
        int earnedStars = 0;
        int maxStars = 0;

        foreach (Level level in levelGroup.levels) {
            State levelState = SavedGameController.instance.getLevelState(level);
            int maxStarsForLevel = level.stars * levelPerfectStarMultiplier;
            maxStars += maxStarsForLevel;

            if (levelState == State.NotOk) {
                continue;
            }

            earnedStars += levelState == State.Perfect ?
                maxStarsForLevel :
                level.stars;
        }

        earnedStarsUi.text = earnedStars.ToString();
        maxStarsUi.text = string.Format("/{0}", maxStars);
    }
}