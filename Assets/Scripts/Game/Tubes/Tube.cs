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
    public Animator tubeAnimator;
    public Animator bulletAnimator;
    public float bulletAnimatorSpeed;
    public SpriteRenderer tubeRenderer;
    public List<Tile> targetTiles = new List<Tile>();

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
    private Dictionary<int, string> shootFromDistanceAnimations = new Dictionary<int, string>();
    private ColorManager.Colors nextColor;

    public bool CanPlay()
    {
        return DistanceFromTargetTile() != -1;
    }

    public int DistanceFromTargetTile()
    {
        for (int i = 0; i < targetTiles.Count; i++)
        {
            if (targetTiles[i].Color != ColorManager.Colors.None)
            {
                return CanMergeOrGrow(targetTiles[i]) ? i : i - 1;
            }
        }

        return Color == ColorManager.Colors.Multicolor ? -1 : targetTiles.Count - 1;
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

        for (int i = 0; i < targetTiles.Count; i++)
        {
            if (targetTiles[i].Color != ColorManager.Colors.None)
            {
                ShowHintForTarget(targetTiles[i]);
                return;
            }
        }
    }

    public void TakeNextColor()
    {
        Color = nextColor;
    }

    private void Awake()
    {
        SetHintLocations();
        SetShootFromDistanceAnimations();
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

    private void SetShootFromDistanceAnimations()
    {
        shootFromDistanceAnimations[0] = "Shoot1TileAway";
        shootFromDistanceAnimations[1] = "Shoot2TilesAway";
        shootFromDistanceAnimations[2] = "Shoot3TilesAway";
    }

    private void PlayBulletAnimation()
    {
        int distance = DistanceFromTargetTile();
        if (!shootFromDistanceAnimations.ContainsKey(distance))
        {
            Debug.Log("Tube::PlayBulletAnimation: no shoot animation found for distance " + distance + ".");
            return;
        }
        bulletAnimator.SetTrigger(shootFromDistanceAnimations[distance]);
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
