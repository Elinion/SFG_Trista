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

	private Tile.TileType type;
	private Score score;

	public delegate void OnLaunchAction ();

	public static event OnLaunchAction OnLaunchEnd;

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
				bulletSprite = yellowBullet;
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
		ChangeType ();
	}

	void Start ()
	{
		bulletAnimator.speed = bulletAnimatorSpeed;
	}

	public void ChangeType ()
	{
		Tile.TileType newType = (Tile.TileType)Random.Range (0, 9);
		Type = newType;
	}

	public void ShowHints ()
	{
		if (Type == Tile.TileType.Rainbomb) {
			return;
		}
		for (int i = 0; i < targetTilesIndexes.Count; i++) {
			int tileIndex = targetTilesIndexes [i];
			Tile tile = Board.instance.tiles [tileIndex];
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
		if (distance != -1) {
			ClickManager.instance.enabled = false;
			ShowBulletAnimation (distance);
			Tile target = Board.instance.tiles [targetTilesIndexes [distance]];
			LaunchOnTile (target);
			LauncherManager.OnShiftEnd += SetRandomType;
			StartCoroutine (OnShootBulletAnimationFinished ((float)distance, target));
		}
	}

	private int DistanceFromValidTarget ()
	{
		int closestTileIndex = Board.instance.boardSize;
		for (int i = targetTilesIndexes.Count - 1; i >= 0; i--) {
			if (Board.instance.tiles [targetTilesIndexes [i]].Type != Tile.TileType.None) {
				closestTileIndex = i;
			}
		}
		if (closestTileIndex == Board.instance.boardSize) {
			return closestTileIndex - 1;
		} 
		Tile target = Board.instance.tiles [targetTilesIndexes [closestTileIndex]];
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
		LauncherManager.OnShiftEnd -= SetRandomType;
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
		float time = distance / bulletAnimator.speed;
		yield return new WaitForSeconds (time);
		yield return new WaitUntil (() => 
			bulletAnimator.GetCurrentAnimatorStateInfo (0).normalizedTime > time
		);
		bulletAnimator.SetTrigger ("Idle");
		tile.Refresh ();
		OnLaunchEnd ();
	}
}
