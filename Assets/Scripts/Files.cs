using System.IO;
using UnityEngine;

public static class Files {
    public static T readFromJson<T>(string fileName) where T : class {
        string filePath = getFilePath(fileName);

#if UNITY_EDITOR || UNITY_IOS
        string data = File.Exists(filePath) ? File.ReadAllText(filePath) : null;

#elif PLATFORM_ANDROID
        WWW reader = new WWW(filePath);
        while (!reader.isDone) {}

        string data = reader.text;
#endif

        if (!string.IsNullOrEmpty(data)) {
            return JsonUtility.FromJson<T>(data);
        }

        Debug.LogError("Failed to get data as json. Cannot find file: " + filePath);
        return null;
    }

    private static string getFilePath(string fileName) {
#if UNITY_EDITOR || UNITY_IOS
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
#elif PLATFORM_ANDROID
        string filePath = "jar:file://" + Application.dataPath + "!/assets/" + fileName;
#endif
        return filePath;
    }
}