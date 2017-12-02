using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor {

    public override void OnInspectorGUI() {
        Level level = (Level)target;

        level.levelName = EditorGUILayout.TextField("Level name", level.levelName);
    }
}
