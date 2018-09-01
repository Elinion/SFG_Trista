using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

public class Tube : MonoBehaviour {
    public enum Direction {
        Up,
        Down,
        Left,
        Right
    }

    public enum ShiftType {
        Linear,
        Angular
    }

    public Direction direction;
    public ShiftType shiftType;
    public Animator tubeAnimator;
    public Animator bulletAnimator;
    public float bulletAnimatorSpeed;
    public SpriteRenderer tubeRenderer;
    public List<Tile> targetTiles = new List<Tile>();

    private Colors color;

    public Colors Color {
        get { return color; }
        set {
            color = value;
            tubeRenderer.sprite = ColorManager.instance.getColorAssets(color).tube;
            bulletAnimator.gameObject.GetComponent<SpriteRenderer>().sprite =
                ColorManager.instance.getColorAssets(color).ball;
        }
    }

    private Dictionary<Direction, Tile.HintLocation> hintLocations = new Dictionary<Direction, Tile.HintLocation>();
    private Dictionary<int, string> shootFromDistanceAnimations = new Dictionary<int, string>();
    private Colors nextColor;

    public bool canPlay() {
        return distanceFromTargetTile() != -1;
    }

    private int distanceFromTargetTile() {
        for (int i = 0; i < targetTiles.Count; i++) {
            if (targetTiles[i].Color != Colors.None) {
                return CanMergeOrGrow(targetTiles[i]) ? i : i - 1;
            }
        }

        return Color == Colors.Multicolor ? -1 : targetTiles.Count - 1;
    }

    public void play() {
        PlayBulletAnimation();
    }

    public void shift(Colors nextColor) {
        switch (shiftType) {
            case ShiftType.Linear:
                tubeAnimator.SetTrigger("Shift");
                break;
            case ShiftType.Angular:
                tubeAnimator.SetTrigger("Spin");
                break;
        }

        this.nextColor = nextColor;
    }

    public void showHints() {
        if (Color == Colors.Multicolor)
            return;

        for (int i = 0; i < targetTiles.Count; i++) {
            if (targetTiles[i].Color != Colors.None) {
                ShowHintForTarget(targetTiles[i]);
                return;
            }
        }
    }

    public void takeNextColor() {
        Color = nextColor;
    }

    private void Awake() {
        SetHintLocations();
        SetShootFromDistanceAnimations();
    }

    private void Start() {
        bulletAnimator.speed = bulletAnimatorSpeed;
    }

    private bool CanMergeOrGrow(Tile target) {
        return target.Color == Color
               || ColorManager.getMergeResult(Color, target.Color) != Colors.None;
    }

    private void SetHintLocations() {
        hintLocations[Direction.Up] = Tile.HintLocation.Bottom;
        hintLocations[Direction.Down] = Tile.HintLocation.Top;
        hintLocations[Direction.Left] = Tile.HintLocation.Right;
        hintLocations[Direction.Right] = Tile.HintLocation.Left;
    }

    private void SetShootFromDistanceAnimations() {
        shootFromDistanceAnimations[0] = "Shoot1TileAway";
        shootFromDistanceAnimations[1] = "Shoot2TilesAway";
        shootFromDistanceAnimations[2] = "Shoot3TilesAway";
    }

    private void PlayBulletAnimation() {
        int distance = distanceFromTargetTile();
        if (!shootFromDistanceAnimations.ContainsKey(distance)) {
            Debug.Log("Tube::PlayBulletAnimation: no shoot animation found for distance " + distance + ".");
            return;
        }

        bulletAnimator.SetTrigger(shootFromDistanceAnimations[distance]);
    }

    private void ShowHintForTarget(Tile target) {
        Colors mergeResultWithTarget = ColorManager.getMergeResult(Color, target.Color);
        if (mergeResultWithTarget != Colors.None) {
            target.showHint(hintLocations[direction], mergeResultWithTarget);
        }
    }
}