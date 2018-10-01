using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameProgress {
    public enum State {
        NotOk,
        Ok,
        Perfect
    }

    [Serializable]
    public abstract class GameElement {
        public string id;
        public State state;
    }

    [Serializable]
    public class WorldByName {
        public string name;
        public World world;
    }

    [Serializable]
    public class Game : GameElement {
        public World[] worlds;
        public LevelGroup[] levelGroups;
        public Level[] levels;

        [NonSerialized] 
        public Dictionary<string, World> worldsById;

        [NonSerialized]
        public Dictionary<string, LevelGroup> levelGroupsById;
        
        [NonSerialized]
        public Dictionary<string, Level> levelsById;
        
        private List<string> synchronizedElements;

        public void init() {
            worldsById = new Dictionary<string, World>();
            if (worlds != null) {
                foreach (World world in worlds) {
                    worldsById.Add(world.id, world);
                }
            }

            levelGroupsById = new Dictionary<string, LevelGroup>();
            if (levelGroups != null) {
                foreach (LevelGroup levelGroup in levelGroups) {
                    levelGroupsById.Add(levelGroup.id, levelGroup);
                }
            }

            levelsById = new Dictionary<string, Level>();
            if (levels != null) {
                foreach (Level level in levels) {
                    levelsById.Add(level.id, level);
                }
            }

            synchronizedElements = new List<string>();
        }

        public void toSaveFormat() {
            worlds = worldsById.Values.ToArray();
            levelGroups = levelGroupsById.Values.ToArray();
            levels = levelsById.Values.ToArray();
        }

        public byte[] toSaveFormatAsBytes() {
//            return Encoding.Default.GetBytes(toSaveFormat());
            return null;
        }

        public static Game fromSaveFormat(byte[] gameProgressBytes) {
            string gameProgressAsJson = Encoding.Default.GetString(gameProgressBytes);
            return JsonUtility.FromJson<Game>(gameProgressAsJson);
        }

        public void markAsSynchronized(string elementId) {
            if (synchronizedElements == null) {
                synchronizedElements = new List<string>();
            }

            if (!synchronizedElements.Contains(elementId)) {
                synchronizedElements.Add(elementId);
            }
        }

        public void removeUnsynchronizedElements() {
            removeUnsynchronizedElements(worldsById);
            removeUnsynchronizedElements(levelGroupsById);
            removeUnsynchronizedElements(levelsById);
            synchronizedElements.Clear();
        }

        private void removeUnsynchronizedElements<T>(Dictionary<string, T> gameElements) {
            List<string> elementsToRemove = new List<string>();
            foreach (string id in gameElements.Keys) {
                if (!synchronizedElements.Contains(id)) {
                    elementsToRemove.Add(id);
                }
            }

            foreach (string elementId in elementsToRemove) {
                gameElements.Remove(elementId);
            }
        }
    }

    [Serializable]
    public class World : GameElement {}

    [Serializable]
    public class LevelGroup : GameElement {}

    [Serializable]
    public class Level : GameElement {
    }
}