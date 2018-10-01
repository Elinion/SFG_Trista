using GameData;
using UnityEngine;

public class DataController : MonoBehaviour {
    public static DataController instance;

    public World[] worlds { get; private set; }

    private const string GameDataFileName = "data.json";

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        loadGameData();
    }

    private void loadGameData() {
        Debug.Log("DataController:load game data");
        Game gameData = Files.readFromJson<Game>(GameDataFileName);
        ByIndexAccessor.loadGame(gameData);
        worlds = gameData.worlds;
    }

    public World getWorldById(int worldId) {
        if (worldId < 0 || worldId >= worlds.Length) {
            Debug.Log("There is no world that matches the requested world id: " + worldId);
        }

        return worlds[worldId];
    }
}