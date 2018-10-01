using System;

namespace GameData {
    [Serializable]
    public abstract class GameElement {
        public string name;
        public string id;
        public int index;
    }

    [Serializable]
    public class Game {
        public World[] worlds;
    }

    [Serializable]
    public class World : GameElement {
        public LevelGroup[] levelGroups;
    }

    [Serializable]
    public class LevelGroup : GameElement {
        public Level[] levels;
    }

    [Serializable]
    public class Level : GameElement {
        public bool rotateAfterPlay;
        public bool enableTileValues;
        public Colors[] defaultTubeColors;
        public Colors[] tubeColors;
        public Tile[] tiles;
        public int stars = 1;
        public int extraTilesAllowedForPerfect = 0;
        public string sceneAdditiveName;
    }

    [Serializable]
    public class Tile {
        public Colors color;
        public Colors defaultColor;
        public int minimumValue;
        public bool required;
    }
}