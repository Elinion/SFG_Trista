using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class DataController : MonoBehaviour {
    public static DataController instance = null;

    public bool loadMainMenuOnStart = true;

    private WorldData[] worlds;

    private const string GameDataFileName = "data.json";

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this)
        {
            Destroy (gameObject);
        }

        DontDestroyOnLoad (gameObject);
        loadGameData();
        if (loadMainMenuOnStart) {
            SceneManager.LoadScene("mainMenu");
        }
    }

    private void loadGameData() {
        string filePath = Path.Combine(Application.streamingAssetsPath, GameDataFileName);
        if (File.Exists(filePath)) {
            string dataAsJson = File.ReadAllText(filePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);
            worlds = loadedData.worlds;
        } else {
            Debug.LogError("Cannot load game data.");
        }
    }

    public WorldData getWorldById(int worldId) {
        if (worldId < 0 || worldId >= worlds.Length) {
            Debug.Log("There is no world that matches the requested world id: " + worldId);
        }

        return worlds[worldId];
    }
}