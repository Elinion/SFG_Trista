using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherManager : MonoBehaviour
{
	public List<Launcher> launchers = new List<Launcher> ();

	void Start ()
	{
		UpdateHints ();
	}

	public void ShiftLaunchers ()
	{
		Tile.TileType toShift = launchers [launchers.Count - 1].Type;
		for (int i = 0; i < launchers.Count; i++) {
			Tile.TileType temp = launchers [i].Type;
			launchers [i].Type = toShift;
			toShift = temp;
		}
	}

	public void UpdateHints ()
	{
		foreach (Launcher launcher in launchers) {
			launcher.ShowHints ();
		}
	}
}
