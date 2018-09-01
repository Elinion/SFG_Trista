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
        Game gameData = Files.readFromJson<Game>(GameDataFileName);
        Debug.Log("DataController:load game data");
        ByOrderIndexAccessor.loadGame(gameData);
        Debug.Log("Number of worlds: " + gameData.worlds.Length);
        worlds = gameData.worlds;
    }

    public World getWorldById(int worldId) {
        if (worldId < 0 || worldId >= worlds.Length) {
            Debug.Log("There is no world that matches the requested world id: " + worldId);
        }

        return worlds[worldId];
    }
}