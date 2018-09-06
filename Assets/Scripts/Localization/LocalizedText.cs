using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour {
    public enum StringFormat {
        None,
        Lowercase,
        Uppercase,
        Capitalized
    }

    public string key;
    public StringFormat stringFormat = StringFormat.None;

    void Start() {
        if (string.IsNullOrEmpty(key)) {
            key = GetComponent<Text>().text;
        }
        
        string localizedValue = LocalizationController.instance.getLocalizedValue(key);
        localizedValue = getFormattedText(localizedValue, stringFormat);

        Text text = GetComponent<Text>();
        text.text = localizedValue;
    }

    private static string getFormattedText(string text, StringFormat format) {
        switch (format) {
            case StringFormat.None:
                return text;
            case StringFormat.Lowercase:
                return text.ToLower();
            case StringFormat.Uppercase:
                return text.ToUpper();
            case StringFormat.Capitalized:
                return text.First().ToString().ToUpper() + text.Substring(1);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}