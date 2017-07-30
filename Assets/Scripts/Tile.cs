using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public enum TileType
	{
		Black,
		Blue,
		Green,
		Orange,
		Purple,
		Red,
		White,
		Yellow,
		Rainbomb,
		Gray,
		ClockwiseSpin,
		CounterClockwiseSpin,
		None
	}

	public TileType defaultColor;
	public Sprite black;
	public Sprite blackTriangle;
	public Sprite blue;
	public Sprite blueTriangle;
	public Sprite gray;
	public Sprite grayTriangle;
	public Sprite green;
	public Sprite greenTriangle;
	public Sprite orange;
	public Sprite orangeTriangle;
	public Sprite purple;
	public Sprite purpleTriangle;
	public Sprite red;
	public Sprite redTriangle;
	public Sprite white;
	public Sprite whiteTriangle;
	public Sprite yellow;
	public Sprite yellowTriangle;
	public GameObject topHint;
	public GameObject rightHint;
	public GameObject bottomHint;
	public GameObject leftHint;
	public TextMesh levelText;

	private TileType type = TileType.None;

	public TileType Type {
		get { return type; }
		set {
			type = value;
			Sprite sprite;
			switch (value) {
			case TileType.Black:
				sprite = black;
				break;
			case TileType.Blue:
				sprite = blue;
				break;
			case TileType.Gray:
				sprite = gray;
				break;
			case TileType.Green:
				sprite = green;
				break;
			case TileType.Orange:
				sprite = orange;
				break;
			case TileType.Purple:
				sprite = purple;
				break;
			case TileType.Red:
				sprite = red;
				break;
			case TileType.White:
				sprite = white;
				break;
			case TileType.Yellow:
				sprite = yellow;
				break;
			default:
				sprite = null;
				break;
			}
			GetComponent<SpriteRenderer> ().sprite = sprite;
		}
	}

	private int level = 1;

	public int Level {
		get { return level; }
	}

	void Awake ()
	{
		Type = defaultColor;
		level = 1;
		levelText.text = "";
	}

	public TileType MergeResult (TileType otherType)
	{
		if (Type == TileType.Gray) {
			return otherType;
		}
		if (Type == TileType.Blue && otherType == TileType.Yellow
		    || Type == TileType.Yellow && otherType == TileType.Blue) {
			return TileType.Green;
		}
		if (
			Type == TileType.Yellow && otherType == TileType.Red
			|| Type == TileType.Red && otherType == TileType.Yellow) {
			return TileType.Orange;
		}
		if (
			Type == TileType.Red && otherType == TileType.Blue
			|| Type == TileType.Blue && otherType == TileType.Red) {
			return TileType.Purple;
		}
		if (
			Type == TileType.Black && otherType == TileType.White
			|| Type == TileType.White && otherType == TileType.Black) {
			return TileType.Gray;
		}
		return TileType.None;
	}

	public void Merge (TileType otherType)
	{
		if (Type == TileType.Gray) {
			Type = otherType;
		} else if (Type == TileType.Blue && otherType == TileType.Yellow
		           || Type == TileType.Yellow && otherType == TileType.Blue) {
			Type = TileType.Green;
		} else if (Type == TileType.Yellow && otherType == TileType.Red
		           || Type == TileType.Red && otherType == TileType.Yellow) {
			Type = TileType.Orange;
		} else if (Type == TileType.Red && otherType == TileType.Blue
		           || Type == TileType.Blue && otherType == TileType.Red) {
			Type = TileType.Purple;
		} else if (Type == TileType.Black && otherType == TileType.White
		           || Type == TileType.White && otherType == TileType.Black) {
			Type = TileType.Gray;
		} 
	}

	public void Grow ()
	{
		level++;
		levelText.text = level > 1 ? level.ToString () : "";
	}

	public void HideHints ()
	{
		topHint.SetActive (false);
		rightHint.SetActive (false);
		bottomHint.SetActive (false);
		leftHint.SetActive (false);
	}

	public void ResetLevel ()
	{
		level = 1;
		levelText.text = "";
	}

	public void Put (TileType type)
	{
		Type = type;
	}

	public void ShowTopHint (TileType type)
	{
		ShowHint (topHint, type);
	}

	public void ShowRightHint (TileType type)
	{
		ShowHint (rightHint, type);
	}

	public void ShowBottomHint (TileType type)
	{
		ShowHint (bottomHint, type);
	}

	public void ShowLeftHint (TileType type)
	{
		ShowHint (leftHint, type);
	}

	private void ShowHint (GameObject hint, TileType type)
	{
		hint.SetActive (true);
		hint.GetComponent<SpriteRenderer> ().sprite = TypeToHintSprite (type);
	}

	private Sprite TypeToHintSprite (TileType type)
	{
		switch (type) {
		case TileType.Black:
			return blackTriangle;
		case TileType.Blue:
			return blueTriangle;
		case TileType.Green:
			return greenTriangle;
		case TileType.Orange:
			return orangeTriangle;
		case TileType.Purple:
			return purpleTriangle;
		case TileType.Red:
			return redTriangle;
		case TileType.White:
			return whiteTriangle;
		case TileType.Yellow:
			return yellowTriangle;
		case TileType.Gray:
			return grayTriangle;
		}
		return null;
	}
}
