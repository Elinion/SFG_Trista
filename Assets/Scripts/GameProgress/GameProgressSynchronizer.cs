using System;
using GameProgress;
using UnityEngine;
using Level = GameData.Level;
using LevelGroup = GameData.LevelGroup;
using World = GameData.World;

public class GameProgressSynchronizer {
    private const State DefaultState = State.NotOk;
    private const State BestState = State.Perfect;

    public void synchronizeGameDataAndProgress(Game gameProgress) {
        gameProgress.init();
        World[] worldsData = DataController.instance.worlds;

        State gameState = BestState;
        foreach (World world in worldsData) {
            if (!gameProgress.worldsById.ContainsKey(world.id)) {
                gameProgress.worldsById.Add(world.id, new GameProgress.World {id = world.id, state = DefaultState});
                gameState = getGlobalState(gameState, DefaultState);
            }

            State worldState = synchronizeWorldDataAndProgress(gameProgress, world);
            gameState = getGlobalState(gameState, worldState);
            gameProgress.markAsSynchronized(world.id);
        }

        gameProgress.removeUnsynchronizedElements();
        gameProgress.state = gameState;
    }

    private State synchronizeWorldDataAndProgress(Game gameProgress, World world) {
        State worldState = BestState;
        foreach (LevelGroup levelGroup in world.levelGroups) {
            if (!gameProgress.levelGroupsById.ContainsKey(levelGroup.id)) {
                gameProgress.levelGroupsById.Add(levelGroup.id,
                    new GameProgress.LevelGroup {id = levelGroup.id, state = DefaultState});
                worldState = getGlobalState(worldState, DefaultState);
            }

            State levelGroupState = synchronizeLevelsDataAndProgress(gameProgress, levelGroup);
            worldState = getGlobalState(worldState, levelGroupState);

            gameProgress.worldsById[world.id].state = worldState;
            gameProgress.markAsSynchronized(levelGroup.id);
        }

        return worldState;
    }

    private State synchronizeLevelsDataAndProgress(Game gameProgress, LevelGroup levelGroup) {
        State levelGroupState = BestState;
        foreach (Level level in levelGroup.levels) {
            if (!gameProgress.levelsById.ContainsKey(level.id)) {
                gameProgress.levelsById.Add(level.id, new GameProgress.Level {id = level.id, state = DefaultState});
                levelGroupState = getGlobalState(levelGroupState, DefaultState);
            }

            State levelState = gameProgress.levelsById[level.id].state;
            levelGroupState = getGlobalState(levelGroupState, levelState);
            gameProgress.markAsSynchronized(level.id);
        }

        gameProgress.levelGroupsById[levelGroup.id].state = levelGroupState;
        return levelGroupState;
    }

    private State getGlobalState(State currentGlobalState, State newElementState) {
        switch (newElementState) {
            case State.NotOk: return State.NotOk;
            case State.Ok: return currentGlobalState == State.NotOk ? State.NotOk : newElementState;
            case State.Perfect: return currentGlobalState == State.Perfect ? newElementState : currentGlobalState;
            default:
                throw new ArgumentOutOfRangeException("newElementState", newElementState, null);
        }
    }
}