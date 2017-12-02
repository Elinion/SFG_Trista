using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class DataController : MonoBehaviour {

    public bool loadMainMenuOnStart = true;

    private LevelGroupData[] allLevelGroups;

    public LevelGroupData[] LevelGroups {
        get { return allLevelGroups; }
    }

    private string gameDataFileName = "data.json";

	void Awake () { 
        DontDestroyOnLoad(gameObject);
        LoadGameData();

        if(loadMainMenuOnStart) {
            SceneManager.LoadScene("mainMenu");
        }
    }

    private void LoadGameData() {
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);
        if(File.Exists(filePath)) {
            string dataAsJson = File.ReadAllText(filePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);
            allLevelGroups = loadedData.allLevelGroups;
        } else {
            Debug.LogError("Cannot load game data.");
        }
    }
}
