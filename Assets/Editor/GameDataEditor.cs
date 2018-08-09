﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class GameDataEditor : EditorWindow {

    public GameData gameData;

    private const string GameDataProjectFilePath = "/StreamingAssets/data.json";

    [MenuItem ("Window/Game Data Editor")]
    static void Init() {
        EditorWindow.GetWindow(typeof(GameDataEditor)).Show();
    }

    private void OnGUI()
    {
        if(gameData != null) {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("gameData");

            EditorGUILayout.PropertyField(serializedProperty, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            serializedObject.ApplyModifiedProperties();

            if(GUILayout.Button("Save data")) {
                SaveGameData();
            }
        }

        if(GUILayout.Button("Load data")) {
            LoadGameData();
        }
    }

    private void LoadGameData() {
        string filePath = Application.dataPath + GameDataProjectFilePath;
        if(File.Exists(filePath)) {
            string dataAsJson = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(dataAsJson);
        } else {
            gameData = new GameData();
        }
    }

    private void SaveGameData() {
        string dataAsJson = JsonUtility.ToJson(gameData);
        string filePath = Application.dataPath + GameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);
    }
}
