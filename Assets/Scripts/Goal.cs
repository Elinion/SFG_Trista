using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
	public List<Image> targetTilesUI = new List<Image> ();
    public Sprite transparent;

	void Start ()
	{
		LevelColors.LevelParams level = GetComponent<LevelColors> ().Level;
		for (int i = 0; i < 9; i++) {

            targetTilesUI[i].sprite = level.pattern[i] == ColorManager.Colors.None ?
                transparent :
                ColorManager.instance.GetColorAssets(level.pattern[i]).tile;
		}
	}
}
