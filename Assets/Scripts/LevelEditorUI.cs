using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[ExecuteInEditMode]
public class LevelEditorUI : MonoBehaviour {

    public Level level;
    public Text levelName;

    private void Update() {
        levelName.text = level.levelName;
        Debug.Log("updating");
    }
}
