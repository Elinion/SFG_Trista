using System.Collections.Generic;
using UnityEngine;

namespace GameData {
    public static class ByIndexAccessor {
        private static Dictionary<int, World> worldsByIndex;
        private static Dictionary<int, Dictionary<int, LevelGroup>> levelGroupsByIndex;
        private static Dictionary<int, Dictionary<int, Dictionary<int, Level>>> levelsByIndex;

        public static World getWorld(int worldIndex) {
            return worldsByIndex.ContainsKey(worldIndex) ? worldsByIndex[worldIndex] : null;
        }

        public static LevelGroup getLevelGroup(int worldIndex, int levelGroupIndex) {
            if (levelGroupsByIndex.ContainsKey(worldIndex)
                && levelGroupsByIndex[worldIndex].ContainsKey(levelGroupIndex)) {
                return levelGroupsByIndex[worldIndex][levelGroupIndex];
            }

            return null;
        }

        public static Level getLevel(int worldIndex, int levelGroupIndex, int levelIndex) {
            if (levelsByIndex.ContainsKey(worldIndex)
                && levelsByIndex[worldIndex].ContainsKey(levelGroupIndex)
                && levelsByIndex[worldIndex][levelGroupIndex].ContainsKey(levelIndex)) {
                return levelsByIndex[worldIndex][levelGroupIndex][levelIndex];
            }

            return null;
        }

        public static void loadGame(Game game) {
            worldsByIndex = new Dictionary<int, World>();
            levelGroupsByIndex = new Dictionary<int, Dictionary<int, LevelGroup>>();
            levelsByIndex = new Dictionary<int, Dictionary<int, Dictionary<int, Level>>>();

            foreach (World world in game.worlds) {
                worldsByIndex.Add(world.index, world);
                setUpLevelGroupsByIndex(world.index, world.levelGroups);
            }
        }

        private static void setUpLevelGroupsByIndex(int worldIndex, LevelGroup[] levelGroups) {
            if (!levelGroupsByIndex.ContainsKey(worldIndex)) {
                levelGroupsByIndex.Add(worldIndex, new Dictionary<int, LevelGroup>());
            }

            foreach (LevelGroup levelGroup in levelGroups) {
                if (!levelGroupsByIndex[worldIndex].ContainsKey(levelGroup.index)) {
                    levelGroupsByIndex[worldIndex].Add(levelGroup.index, levelGroup);
                }

                setUpLevelsByIndex(worldIndex, levelGroup.index, levelGroup.levels);
            }
        }

        private static void setUpLevelsByIndex(int worldIndex, int levelGroupIndex, Level[] levels) {
            if (!levelsByIndex.ContainsKey(worldIndex)) {
                levelsByIndex.Add(worldIndex, new Dictionary<int, Dictionary<int, Level>>());
            }

            if (!levelsByIndex[worldIndex].ContainsKey(levelGroupIndex)) {
                levelsByIndex[worldIndex].Add(levelGroupIndex, new Dictionary<int, Level>());
            }

            foreach (Level level in levels) {
                if (!levelsByIndex[worldIndex][levelGroupIndex].ContainsKey(level.index)) {
                    levelsByIndex[worldIndex][levelGroupIndex].Add(level.index, level);
                }
            }
        }
    }
}