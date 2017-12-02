using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
    public enum Direction
    {
        Up, Down, Left, Right
    }

    public enum ShiftType
    {
        Linear, Angular
    }

    public Direction direction;
    public ShiftType shiftType;
    public Tile firstTarget;
    public Tile secondTarget;
    public Tile thirdTarget;
    public Animator tubeAnimator;
    public Animator bulletAnimator;
    public float bulletAnimatorSpeed;
    public SpriteRenderer tubeRenderer;

    private ColorManager.Colors color;
    public ColorManager.Colors Color
    {
        get { return color; }
        set
        {
            color = value;
            tubeRenderer.sprite = ColorManager.instance.GetColorAssets(color).tube;
            bulletAnimator.gameObject.GetComponent<SpriteRenderer>().sprite = ColorManager.instance.GetColorAssets(color).ball;
        }
    }

    private Dictionary<Direction, Tile.HintLocation> hintLocations = new Dictionary<Direction, Tile.HintLocation>();
    private ColorManager.Colors nextColor;

    public bool CanPlay()
    {
        return DistanceFromTargetTile() > 0;
    }

    public int DistanceFromTargetTile()
    {
        if (firstTarget.Color != ColorManager.Colors.None)
        {
            return CanMergeOrGrow(firstTarget) ? 1 : 0;
        }
        else if (secondTarget.Color != ColorManager.Colors.None)
        {
            return CanMergeOrGrow(secondTarget) ? 2 : 1;
        }
        else if (thirdTarget.Color != ColorManager.Colors.None)
        {
            return CanMergeOrGrow(thirdTarget) ? 3 : 2;
        }
        else
        {
            return Color == ColorManager.Colors.Multicolor ? 0 : 3;
        }
    }

    public void Play()
    {
        PlayBulletAnimation();
    }

    public void Shift(ColorManager.Colors nextColor)
    {
        switch (shiftType)
        {
            case ShiftType.Linear:
                tubeAnimator.SetTrigger("Shift");
                break;
            case ShiftType.Angular:
                tubeAnimator.SetTrigger("Spin");
                break;
        }
        this.nextColor = nextColor;
    }

    public void ShowHints()
    {
        if (Color == ColorManager.Colors.Multicolor)
            return;

        if (firstTarget.Color != ColorManager.Colors.None)
        {
            ShowHintForTarget(firstTarget);
        }
        else if (secondTarget.Color != ColorManager.Colors.None)
        {
            ShowHintForTarget(secondTarget);
        }
        else if (thirdTarget.Color != ColorManager.Colors.None)
        {
            ShowHintForTarget(thirdTarget);
        }
    }

    public void TakeNextColor()
    {
        Color = nextColor;
    }

    private void Awake()
    {
        SetHintLocations();
    }

    private void Start()
    {
        bulletAnimator.speed = bulletAnimatorSpeed;
    }

    private bool CanMergeOrGrow(Tile target)
    {
        return target.Color == Color
                     || ColorManager.MergeColors(Color, target.Color) != ColorManager.Colors.None;
    }

    private void SetHintLocations()
    {
        hintLocations[Direction.Up] = Tile.HintLocation.Bottom;
        hintLocations[Direction.Down] = Tile.HintLocation.Top;
        hintLocations[Direction.Left] = Tile.HintLocation.Right;
        hintLocations[Direction.Right] = Tile.HintLocation.Left;
    }

    private void PlayBulletAnimation()
    {
        string shootTrigger = "";
        switch (DistanceFromTargetTile())
        {
            case 1:
                shootTrigger = "Shoot1TileAway";
                break;
            case 2:
                shootTrigger = "Shoot2TilesAway";
                break;
            case 3:
                shootTrigger = "Shoot3TilesAway";
                break;
            default:
                break;
        }
        bulletAnimator.SetTrigger(shootTrigger);
    }

    private void ShowHintForTarget(Tile target)
    {
        ColorManager.Colors mergeResultWithTarget = ColorManager.MergeColors(Color, target.Color);
        if (mergeResultWithTarget != ColorManager.Colors.None)
        {
            target.ShowHint(hintLocations[direction], mergeResultWithTarget);
        }
    }
}
