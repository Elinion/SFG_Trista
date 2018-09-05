using System.Collections.Generic;
using UnityEngine;

namespace GameData {
    public static class ByOrderIndexAccessor {
        private static Dictionary<int, World> worldsByOrderIndex;
        private static Dictionary<int, Dictionary<int, LevelGroup>> levelGroupsByOrderIndex;
        private static Dictionary<int, Dictionary<int, Dictionary<int, Level>>> levelsByOrderIndex;

        public static World getWorld(int worldOrderIndex) {
            return worldsByOrderIndex.ContainsKey(worldOrderIndex) ? worldsByOrderIndex[worldOrderIndex] : null;
        }

        public static LevelGroup getLevelGroup(int worldOrderIndex, int levelGroupOrderIndex) {
            if (levelGroupsByOrderIndex.ContainsKey(worldOrderIndex)
                && levelGroupsByOrderIndex[worldOrderIndex].ContainsKey(levelGroupOrderIndex)) {
                return levelGroupsByOrderIndex[worldOrderIndex][levelGroupOrderIndex];
            }

            return null;
        }

        public static Level getLevel(int worldOrderIndex, int levelGroupOrderIndex, int levelOrderIndex) {
            if (levelsByOrderIndex.ContainsKey(worldOrderIndex)
                && levelsByOrderIndex[worldOrderIndex].ContainsKey(levelGroupOrderIndex)
                && levelsByOrderIndex[worldOrderIndex][levelGroupOrderIndex].ContainsKey(levelOrderIndex)) {
                return levelsByOrderIndex[worldOrderIndex][levelGroupOrderIndex][levelOrderIndex];
            }

            return null;
        }

        public static void loadGame(Game game) {
            worldsByOrderIndex = new Dictionary<int, World>();
            levelGroupsByOrderIndex = new Dictionary<int, Dictionary<int, LevelGroup>>();
            levelsByOrderIndex = new Dictionary<int, Dictionary<int, Dictionary<int, Level>>>();

            foreach (World world in game.worlds) {
                worldsByOrderIndex.Add(world.orderIndex, world);
                setUpLevelGroupsByOrderIndex(world.orderIndex, world.levelGroups);
            }
        }

        private static void setUpLevelGroupsByOrderIndex(int worldOrderIndex, LevelGroup[] levelGroups) {
            if (!levelGroupsByOrderIndex.ContainsKey(worldOrderIndex)) {
                levelGroupsByOrderIndex.Add(worldOrderIndex, new Dictionary<int, LevelGroup>());
            }

            foreach (LevelGroup levelGroup in levelGroups) {
                if (!levelGroupsByOrderIndex[worldOrderIndex].ContainsKey(levelGroup.orderIndex)) {
                    levelGroupsByOrderIndex[worldOrderIndex].Add(levelGroup.orderIndex, levelGroup);
                }

                setUpLevelsByOrderIndex(worldOrderIndex, levelGroup.orderIndex, levelGroup.levels);
            }
        }

        private static void setUpLevelsByOrderIndex(int worldOrderIndex, int levelGroupOrderIndex, Level[] levels) {
            if (!levelsByOrderIndex.ContainsKey(worldOrderIndex)) {
                levelsByOrderIndex.Add(worldOrderIndex, new Dictionary<int, Dictionary<int, Level>>());
            }

            if (!levelsByOrderIndex[worldOrderIndex].ContainsKey(levelGroupOrderIndex)) {
                levelsByOrderIndex[worldOrderIndex].Add(levelGroupOrderIndex, new Dictionary<int, Level>());
            }

            foreach (Level level in levels) {
                if (!levelsByOrderIndex[worldOrderIndex][levelGroupOrderIndex].ContainsKey(level.orderIndex)) {
                    levelsByOrderIndex[worldOrderIndex][levelGroupOrderIndex].Add(level.orderIndex, level);
                }
            }
        }
    }
}