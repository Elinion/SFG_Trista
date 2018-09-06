using System.Collections;
using System.Collections.Generic;
using GameData;
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
    public bool isAnimating { get; private set; }

    private readonly Dictionary<HintLocation, GameObject> hints = new Dictionary<HintLocation, GameObject>();
    private Colors color = Colors.None;


    public Colors Color {
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

    private void Awake() {
        Reset();
        SetUpHints();
        GetComponent<Animator>().speed = animatorSpeed;
    }

    public static bool isValid(GameData.Tile expected, Tile actual) {
        return expected.color == actual.Color && expected.minimumValue <= actual.Level;
    }

    public void AddColor(Colors otherColor) {
        if (Color == Colors.None) {
            GainColor(otherColor);
        } else if (otherColor == Color
                   || otherColor == Colors.Multicolor) {
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
        isAnimating = true;
        GetComponent<Animator>().SetTrigger("Flip");
    }

    public void remove() {
        isAnimating = true;
        GetComponent<Animator>().SetTrigger("ShrinkAway");
        Invoke("Reset", 1);
    }

    public void Reset() {
        GetComponent<Animator>().SetTrigger("Idle");
        isAnimating = false;
        Color = Colors.None;
        Level = 1;
    }

    public void showHint(HintLocation hintLocation, Colors color) {
        hints[hintLocation].SetActive(true);
        hints[hintLocation].GetComponent<SpriteRenderer>().sprite =
            ColorManager.instance.getColorAssets(color).triangle;
    }

    private void GainColor(Colors otherColor) {
        Color = otherColor;
    }

    private void Merge(Colors otherColor) {
        Colors mergeResult = ColorManager.getMergeResult(color, otherColor);
        if (mergeResult != Colors.None) {
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