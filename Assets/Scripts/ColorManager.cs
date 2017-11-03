using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [System.Serializable]
    public class ColorAssets
    {
        public Sprite ball;
        public Sprite tile;
        public Sprite triangle;
        public Sprite tube;
    }

    public enum Colors
    {
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

    private Dictionary<Colors, ColorAssets>  colorAssets = new Dictionary<Colors, ColorAssets>();

    void Start() {
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
    }

    public ColorAssets GetColorAssets(Colors color) {
        return colorAssets[color];
    }
}
