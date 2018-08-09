using UnityEditor;
using UnityEngine;

public class DeleteAllPlayerPrefsUtil : Editor {
    
    [MenuItem("Utils/Delete All PlayerPrefs")]
    static void DeleteAllPlayerPrefs() {
        PlayerPrefs.DeleteAll();
    }
}