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
	public Sprite blue;
	public Sprite gray;
	public Sprite green;
	public Sprite orange;
	public Sprite purple;
	public Sprite red;
	public Sprite white;
	public Sprite yellow;

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

	void Start ()
	{
		Type = defaultColor;
	}

	public bool Merge (TileType otherType)
	{
		if (Type == TileType.Blue && otherType == TileType.Yellow
		    || Type == TileType.Yellow && otherType == TileType.Blue) {
			Debug.Log ("merged yellow and blue -> green");
			Type = TileType.Green;
		} else if (Type == TileType.Yellow && otherType == TileType.Red
		           || Type == TileType.Red && otherType == TileType.Yellow) {
			Debug.Log ("merged yellow and red -> orange");
			Type = TileType.Orange;
		} else if (Type == TileType.Red && otherType == TileType.Blue
		           || Type == TileType.Blue && otherType == TileType.Red) {
			Debug.Log ("merged blue and red -> purple");
			Type = TileType.Purple;
		} else if (Type == TileType.Black && otherType == TileType.White
		           || Type == TileType.White && otherType == TileType.Black) {
			Debug.Log ("merged black and white -> gray");
			Type = TileType.Gray;
		} else {
			Debug.Log ("could not merge colors");
			return false;
		}
		return true;
	}

	public void Pop ()
	{
		Debug.Log ("Popped");
		Type = TileType.None;
	}

	public void Put (TileType type)
	{
		Debug.Log ("Put " + type.ToString ());
		Type = type;
	}
}
