using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
	public int position;
	public Animator animator;
	public Animator bulletAnimator;
	public float bulletAnimatorSpeed;
	public List<int> targetTilesIndexes = new List<int> ();
	public Sprite black;
	public Sprite blackBullet;
	public Sprite blue;
	public Sprite blueBullet;
	public Sprite green;
	public Sprite greenBullet;
	public Sprite orange;
	public Sprite orangeBullet;
	public Sprite purple;
	public Sprite purpleBullet;
	public Sprite red;
	public Sprite redBullet;
	public Sprite white;
	public Sprite whiteBullet;
	public Sprite yellow;
	public Sprite yellowBullet;
	public Sprite rainbomb;
    public Sprite rainbombBullet;

	private Tile.TileType type;
	private Score score;
	private Board board;
	private LevelColors levelColors;

	public delegate void OnLaunchAction ();

	public event OnLaunchAction OnLaunchEnd;

	public Tile.TileType Type {
		get { return type; }
		set {
			type = value;
			Sprite sprite;
			Sprite bulletSprite;
			switch (value) {
			case Tile.TileType.Black:
				sprite = black;
				bulletSprite = blackBullet;
				break;
			case Tile.TileType.Blue:
				sprite = blue;
				bulletSprite = blueBullet;
				break;
			case Tile.TileType.Green:
				sprite = green;
				bulletSprite = greenBullet;
				break;
			case Tile.TileType.Orange:
				sprite = orange;
				bulletSprite = orangeBullet;
				break;
			case Tile.TileType.Purple:
				sprite = purple;
				bulletSprite = purpleBullet;
				break;
			case Tile.TileType.Red:
				sprite = red;
				bulletSprite = redBullet;
				break;
			case Tile.TileType.White:
				sprite = white;
				bulletSprite = whiteBullet;
				break;
			case Tile.TileType.Yellow:
				sprite = yellow;
				bulletSprite = yellowBullet;
				break;
			case Tile.TileType.Rainbomb:
				sprite = rainbomb;
				bulletSprite = rainbombBullet;
				break;
			default:
				sprite = null;
				bulletSprite = null;
				break;
			}
			GetComponent<SpriteRenderer> ().sprite = sprite;
			bulletAnimator.gameObject.GetComponent<SpriteRenderer> ().sprite = bulletSprite;
		}
	}

	void Awake ()
	{
		score = GameObject.FindGameObjectWithTag (Tags.Score).GetComponent<Score> ();
		board = GameObject.FindGameObjectWithTag (Tags.Board).GetComponent<Board> ();
		levelColors = GameObject.FindGameObjectWithTag (Tags.LevelController).GetComponent<LevelColors> ();
	}

	void Start ()
	{
		ChangeType ();
		bulletAnimator.speed = bulletAnimatorSpeed;
	}

	public void ChangeType ()
	{
		Type = levelColors.getRandomColor ();
	}

	public void ShowHints ()
	{
		if (Type == Tile.TileType.Rainbomb) {
			return;
		}
		for (int i = 0; i < targetTilesIndexes.Count; i++) {
			int tileIndex = targetTilesIndexes [i];
			Tile tile = board.tiles [tileIndex];
			if (tile.Type == Tile.TileType.None) {
				continue;
			}
			Tile.TileType mergeResult = Tile.MergeResult (tile.Type, Type);
			if (mergeResult != Tile.TileType.None) {
				if (position < 3) {
					tile.ShowTopHint (mergeResult);
				} else if (position < 6) { 
					tile.ShowRightHint (mergeResult);
				} else if (position < 9) {
					tile.ShowBottomHint (mergeResult);
				} else {
					tile.ShowLeftHint (mergeResult);
				}

			}
			break;
		}
	}

	public void Trigger ()
	{
		int distance = DistanceFromValidTarget ();

		// If it is a joker launcher (multicolor), there must be at least one target tile to merge with
		// otherwise the player can't play this launcher this turn

		int farthestTileIndex = board.boardSize - 1;
		if (distance == farthestTileIndex && Type == Tile.TileType.Rainbomb) {
			return;
		}
		if (distance != -1) {
			ClickManager.instance.enabled = false;
			ShowBulletAnimation (distance);
			Tile target = board.tiles [targetTilesIndexes [distance]];
			LaunchOnTile (target);
			LauncherManager.instance.OnShiftEnd += SetRandomType;
			StartCoroutine (OnShootBulletAnimationFinished ((float)distance, target));
		}
	}

	public int DistanceFromValidTarget ()
	{
		int closestTileIndex = board.boardSize;
		for (int i = targetTilesIndexes.Count - 1; i >= 0; i--) {
			if (board.tiles [targetTilesIndexes [i]].Type != Tile.TileType.None) {
				closestTileIndex = i;
			}
		}
		if (closestTileIndex == board.boardSize) {
			return closestTileIndex - 1;
		} 
		Tile target = board.tiles [targetTilesIndexes [closestTileIndex]];
		if (IsTargetValid (target.Type)) {
			return closestTileIndex;
		}
		return closestTileIndex - 1;
	}

	private void GrowTile (Tile tile)
	{
		tile.Grow ();
		score.addGrowPoints ();
	}

	private bool IsTargetValid (Tile.TileType targetType)
	{
		return (
		    Type == Tile.TileType.Rainbomb
		    || Type == targetType
		    || targetType == Tile.TileType.None
		    || Tile.MergeResult (Type, targetType) != Tile.TileType.None
		);
	}

	private void LaunchOnTile (Tile tile)
	{
		if (Type == Tile.TileType.Rainbomb || Type == tile.Type) {
			GrowTile (tile);
		} else if (tile.Type == Tile.TileType.None) {
			SetTile (tile);
		} else if (Tile.MergeResult (tile.Type, Type) != Tile.TileType.None) {
			MergeWithTile (tile);
		} 
	}

	private void MergeWithTile (Tile tile)
	{
		tile.Merge (Type);
		score.addMergePoints ();
	}

	private void SetRandomType ()
	{
		ChangeType ();
		LauncherManager.instance.OnShiftEnd -= SetRandomType;
	}

	private void SetTile (Tile tile)
	{
		tile.Type = Type;
		score.addLaunchPoints ();
	}

	private void ShowBulletAnimation (int targetDistance)
	{
		string shootTrigger = "";
		switch (targetDistance) {
		case 0:
			shootTrigger = "Shoot1TileAway";
			break;
		case 1:
			shootTrigger = "Shoot2TilesAway";
			break;
		case 2:
			shootTrigger = "Shoot3TilesAway";
			break;
		default:
			break;
		}
		bulletAnimator.SetTrigger (shootTrigger);
	}

	IEnumerator OnShootBulletAnimationFinished (float distance, Tile tile)
	{
		float waitTime = distance / bulletAnimator.speed;
		yield return new WaitForSeconds (waitTime);
		yield return new WaitUntil (() => 
			bulletAnimator.GetCurrentAnimatorStateInfo (0).normalizedTime > waitTime
		);
		bulletAnimator.SetTrigger ("Idle");
		tile.Refresh ();
		OnLaunchEnd ();
	}
}
