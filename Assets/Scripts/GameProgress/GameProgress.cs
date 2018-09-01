using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameProgress {
    public enum State {
        NotOk,
        Ok,
        Perfect
    }

    [Serializable]
    public abstract class GameElement {
        public State state;
    }

    [Serializable]
    public class Game : GameElement {
        public int score;
        public Dictionary<string, World> worlds { get; set; }
        public Dictionary<string, LevelGroup> levelGroups { get; set; }
        public Dictionary<string, Level> levels { get; set; }
        private List<string> synchronizedElements;

        public void init() {
            if (worlds == null) {
                worlds = new Dictionary<string, World>();
            }

            if (levelGroups == null) {
                levelGroups = new Dictionary<string, LevelGroup>();
            }

            if (levels == null) {
                levels = new Dictionary<string, Level>();
            }

            synchronizedElements = new List<string>();
        }

        public byte[] toSaveFormat() {
            string gameProgressAsJson = JsonConvert.SerializeObject(this);
            return Encoding.Default.GetBytes(gameProgressAsJson);
        }

        public static Game fromSaveFormat(byte[] gameProgressBytes) {
            string gameProgressAsJson = Encoding.Default.GetString(gameProgressBytes);
            return JsonConvert.DeserializeObject<Game>(gameProgressAsJson);
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
            foreach (string id in worlds.Keys) {
                if (!synchronizedElements.Contains(id)) {
                    worlds.Remove(id);
                }
            }

            foreach (string id in levelGroups.Keys) {
                if (!synchronizedElements.Contains(id)) {
                    levelGroups.Remove(id);
                }
            }

            foreach (string id in levels.Keys) {
                if (!synchronizedElements.Contains(id)) {
                    levels.Remove(id);
                }
            }

            synchronizedElements.Clear();
        }
    }

    [Serializable]
    public class World : GameElement {}

    [Serializable]
    public class LevelGroup : GameElement {}

    [Serializable]
    public class Level : GameElement {}
}