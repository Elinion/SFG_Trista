using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public enum HintLocation {
        Top,
        Bottom,
        Left,
        Right
    }

    public int growthIncrement;
    public float animatorSpeed;
    public TextMesh levelText;
    public GameObject topHint;
    public GameObject bottomHint;
    public GameObject leftHint;
    public GameObject rightHint;

    private readonly Dictionary<HintLocation, GameObject> hints = new Dictionary<HintLocation, GameObject>();
    private ColorManager.Colors color = ColorManager.Colors.None;

    public ColorManager.Colors Color {
        get { return color; }
        set {
            color = value;
            GetComponent<SpriteRenderer>().sprite = ColorManager.instance.getColorAssets(color).tile;
        }
    }

    private int level = 1;

    public int Level {
        get { return level; }
        set {
            level = value;
            levelText.text = level > 1 ? level.ToString() : "";
        }
    }

    public static bool isValid(TileData expected, Tile actual) {
        return expected.color == actual.Color && expected.minimumValue <= actual.Level;
    }

    public void AddColor(ColorManager.Colors otherColor) {
        if (Color == ColorManager.Colors.None) {
            GainColor(otherColor);
        } else if (otherColor == Color
                   || otherColor == ColorManager.Colors.Multicolor) {
            Grow();
        } else {
            Merge(otherColor);
        }
    }

    public void hideHints() {
        topHint.SetActive(false);
        rightHint.SetActive(false);
        bottomHint.SetActive(false);
        leftHint.SetActive(false);
    }

    public void playSuccessAnimation() {
        GetComponent<Animator>().SetTrigger("Flip");
    }

    public void remove() {
        GetComponent<Animator>().SetTrigger("ShrinkAway");
        Invoke("Reset", 1);
    }

    public void Reset() {
        GetComponent<Animator>().SetTrigger("Idle");
        Color = ColorManager.Colors.None;
        Level = 1;
    }

    public void showHint(HintLocation hintLocation, ColorManager.Colors color) {
        hints[hintLocation].SetActive(true);
        hints[hintLocation].GetComponent<SpriteRenderer>().sprite =
            ColorManager.instance.getColorAssets(color).triangle;
    }
    
    private void Start() {
        Reset();
        SetUpHints();
        GetComponent<Animator>().speed = animatorSpeed;
    }

    private void GainColor(ColorManager.Colors otherColor) {
        Color = otherColor;
    }

    private void Merge(ColorManager.Colors otherColor) {
        ColorManager.Colors mergeResult = ColorManager.getMergeResult(color, otherColor);
        if (mergeResult != ColorManager.Colors.None) {
            Color = mergeResult;
        }
    }

    private void Grow() {
        Level += growthIncrement;
    }

    private void SetUpHints() {
        hints.Add(HintLocation.Top, topHint);
        hints.Add(HintLocation.Bottom, bottomHint);
        hints.Add(HintLocation.Left, leftHint);
        hints.Add(HintLocation.Right, rightHint);
    }
}