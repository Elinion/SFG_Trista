using System.IO;
using GameData;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameDataEditor : EditorWindow {
    public Game gameData;

    private const string GameDataProjectFilePath = "/StreamingAssets/data.json";

    [MenuItem("Window/Game Data Editor")]
    static void Init() {
        GetWindow(typeof(GameDataEditor)).Show();
    }

    private void OnGUI() {
        if (gameData != null) {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("gameData");

            EditorGUILayout.PropertyField(serializedProperty, true, GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save data")) {
                saveGameData();
            }
        }

        if (GUILayout.Button("Load data")) {
            loadGameData();
        }
    }

    private void loadGameData() {
        string filePath = Application.dataPath + GameDataProjectFilePath;
        if (File.Exists(filePath)) {
            string dataAsJson = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<Game>(dataAsJson);
        } else {
            gameData = new Game();
        }
    }

    private void saveGameData() {
        generateGameElementGuids(gameData);
        string dataAsJson = JsonUtility.ToJson(gameData);
        string filePath = Application.dataPath + GameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);
    }

    private void generateGameElementGuids(Game game) {
        foreach (World world in game.worlds) {
            generateGuidForGameElement(world);

            foreach (LevelGroup levelGroup in world.levelGroups) {
                generateGuidForGameElement(levelGroup);

                foreach (Level level in levelGroup.levels) {
                    generateGuidForGameElement(level);
                }
            }
        }
    }

    private void generateGuidForGameElement(GameElement gameElement) {
        if (string.IsNullOrEmpty(gameElement.id)) {
            gameElement.id = System.Guid.NewGuid().ToString();
        }
    }
}