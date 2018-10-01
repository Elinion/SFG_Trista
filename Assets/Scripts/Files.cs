using System.IO;
using UnityEngine;

public static class Files {
    public static T readFromJson<T>(string fileName) where T : class {
        string filePath = getFilePath(fileName);

#if UNITY_EDITOR
        string data = File.Exists(filePath) ? File.ReadAllText(filePath) : null;

#elif UNITY_IOS
        StreamReader streamReader = new StreamReader(filePath);
        string data = streamReader.ReadToEnd();

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

    public static void writeAsJson(string fileName, object data) {
        string filePath = getFilePath(fileName);
        string dataAsJson = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, dataAsJson);
    }

    private static string getFilePath(string fileName) {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        Debug.Log("Files requested path: " + filePath);
        return filePath;
    }
}