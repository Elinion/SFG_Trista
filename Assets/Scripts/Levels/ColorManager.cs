using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour {
    public static ColorManager instance;

    [System.Serializable]
    public class ColorAssets {
        public Sprite ball;
        public Sprite tile;
        public Sprite triangle;
        public Sprite tube;
    }

    public enum Colors {
        Black,
        Blue,
        Gray,
        Green,
        Multicolor,
        None,
        Orange,
        Purple,
        Red,
        White,
        Yellow
    }

    public ColorAssets black;
    public ColorAssets blue;
    public ColorAssets gray;
    public ColorAssets green;
    public ColorAssets multicolor;
    public ColorAssets orange;
    public ColorAssets purple;
    public ColorAssets red;
    public ColorAssets white;
    public ColorAssets yellow;
    public ColorAssets transparent;

    private readonly Dictionary<Colors, ColorAssets> colorAssets = new Dictionary<Colors, ColorAssets>();

    private static readonly Colors[] throwableColors = {
        Colors.Black, Colors.Blue, Colors.Green, Colors.Orange, Colors.Purple, Colors.Red, Colors.White, Colors.Yellow,
        Colors.Multicolor
    };

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            setColorAssets();
            DontDestroyOnLoad(gameObject);
        }
    }

    public ColorAssets getColorAssets(Colors color) {
        return colorAssets[color];
    }

    public static Colors getRandomThrowableColor() {
        return throwableColors[Random.Range(0, throwableColors.Length)];
    }

    public static Colors getRandomColorFromArray(Colors[] colorOptions) {
        return colorOptions[Random.Range(0, colorOptions.Length)];
    }

    public static Colors getMergeResult(Colors sourceColor, Colors targetColor) {
        if (sourceColor == targetColor) {
            return Colors.None;
        }

        // Multicolor is the joker color, its color always matches the target
        if (sourceColor == Colors.Multicolor
            && targetColor != Colors.None) {
            return sourceColor;
        }

        // Gray is the neutral color, it always take the other colors
        if (sourceColor == Colors.Gray)
            return targetColor;

        if (targetColor == Colors.Gray)
            return Colors.Gray;

        // Yellow and Blue gives Green
        if (sourceColor == Colors.Blue && targetColor == Colors.Yellow
            || sourceColor == Colors.Yellow && targetColor == Colors.Blue) {
            return Colors.Green;
        }

        // Yellow and Red gives Orange
        if (sourceColor == Colors.Red && targetColor == Colors.Yellow
            || sourceColor == Colors.Yellow && targetColor == Colors.Red) {
            return Colors.Orange;
        }

        // Red and Blue gives Purple
        if (sourceColor == Colors.Blue && targetColor == Colors.Red
            || sourceColor == Colors.Red && targetColor == Colors.Blue) {
            return Colors.Purple;
        }

        // Black and White gives Gray, the neutral color
        if (sourceColor == Colors.Black && targetColor == Colors.White
            || sourceColor == Colors.White && targetColor == Colors.Black) {
            return Colors.Gray;
        }

        return Colors.None;
    }

    private void setColorAssets() {
        colorAssets.Add(Colors.Black, black);
        colorAssets.Add(Colors.Blue, blue);
        colorAssets.Add(Colors.Gray, gray);
        colorAssets.Add(Colors.Green, green);
        colorAssets.Add(Colors.Multicolor, multicolor);
        colorAssets.Add(Colors.Orange, orange);
        colorAssets.Add(Colors.Purple, purple);
        colorAssets.Add(Colors.Red, red);
        colorAssets.Add(Colors.White, white);
        colorAssets.Add(Colors.Yellow, yellow);
        colorAssets.Add(Colors.None, transparent);
    }
}