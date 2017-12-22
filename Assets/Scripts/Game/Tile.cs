using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TilePosition {
        
    }

    public enum HintLocation
    {
        Top, Bottom, Left, Right
    }

    public float animatorSpeed;
    public TextMesh levelText;
    public GameObject topHint;
    public GameObject bottomHint;
    public GameObject leftHint;
    public GameObject rightHint;

    private readonly Dictionary<HintLocation, GameObject> hints = new Dictionary<HintLocation, GameObject>();
    private LevelController levelController;

    private ColorManager.Colors color = ColorManager.Colors.None;
    public ColorManager.Colors Color
    {
        get { return color; }
        set
        {
            color = value;
            GetComponent<SpriteRenderer>().sprite = ColorManager.instance.GetColorAssets(color).tile;
        }
    }

    private int level = 1;
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            levelText.text = level > 1 ? level.ToString() : "";
        }
    }

    public void AddColor(ColorManager.Colors otherColor)
    {
        if (Color == ColorManager.Colors.None)
        {
            GainColor(otherColor);
        }
        else if (otherColor == Color
                 || otherColor == ColorManager.Colors.Multicolor)
        {
            Grow();
        }
        else
        {
            Merge(otherColor);
        }
    }

    public void DoneRemoving()
    {
        Reset();
        levelController.TileHasBeenRemoved(this);
    }

    public void HideHints()
    {
        topHint.SetActive(false);
        rightHint.SetActive(false);
        bottomHint.SetActive(false);
        leftHint.SetActive(false);
    }

    public void PlaySuccessAnimation() {
        GetComponent<Animator>().SetTrigger("Flip");
    }

    public void Remove()
    {
        GetComponent<Animator>().SetTrigger("ShrinkAway");
    }

    public void Reset()
    {
        GetComponent<Animator>().SetTrigger("Idle");
        Color = ColorManager.Colors.None;
        Level = 1;
    }

    public void ShowHint(HintLocation hintLocation, ColorManager.Colors color)
    {
        hints[hintLocation].SetActive(true);
        hints[hintLocation].GetComponent<SpriteRenderer>().sprite = ColorManager.instance.GetColorAssets(color).triangle;
    }

    private void Awake()
    {
        Reset();
        SetUpHints();
        levelController = GameObject.FindGameObjectWithTag(Tags.LevelController).GetComponent<LevelController>();
    }

    private void Start()
    {
        GetComponent<Animator>().speed = animatorSpeed;
    }

    private void GainColor(ColorManager.Colors otherColor)
    {
        Color = otherColor;
    }

    private void Merge(ColorManager.Colors otherColor)
    {
        ColorManager.Colors mergeResult = ColorManager.MergeColors(color, otherColor);
        if (mergeResult != ColorManager.Colors.None)
        {
            Color = mergeResult;
        }
    }

    private void Grow()
    {
        Level++;
    }

    private void SetUpHints()
    {
        hints.Add(HintLocation.Top, topHint);
        hints.Add(HintLocation.Bottom, bottomHint);
        hints.Add(HintLocation.Left, leftHint);
        hints.Add(HintLocation.Right, rightHint);
    }

}
