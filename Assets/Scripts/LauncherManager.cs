using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherManager : MonoBehaviour
{
	public static LauncherManager instance = null;

	public List<Launcher> launchers = new List<Launcher> ();
	public List<Transform> launcherHolders = new List<Transform> ();
	public float launcherAnimatorSpeed;
	private Board board;

	public delegate void OnShiftEndAction ();

	public event OnShiftEndAction OnShiftEnd;

	void Awake ()
	{
		ImplementSingleton ();
		board = GameObject.FindGameObjectWithTag (Tags.Board).GetComponent<Board> ();
	}

	void Start ()
	{
		UpdateHints ();
		board.OnRemoveTriplesEnd += ShiftLaunchers;
		foreach (Launcher launcher in launchers) {
			launcher.GetComponent<Animator> ().speed = launcherAnimatorSpeed;
		}
	}

	public void UnsubscribeFromEvents ()
	{
		board.OnRemoveTriplesEnd -= ShiftLaunchers;
	}

	private void ChangeLaunchersTargets ()
	{
		List<int> toShift = launchers [0].targetTilesIndexes;
		for (int i = launchers.Count - 1; i >= 0; i--) {
			List<int> temp = launchers [i].targetTilesIndexes;
			launchers [i].targetTilesIndexes = toShift;
			toShift = temp;
		}
	}

	private void ImplementSingleton ()
	{
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	private void MoveLaunchers ()
	{
		foreach (Launcher launcher in launchers) {
			string motion = "";
			switch (launcher.position) {
			case 2:
			case 5:
			case 8:
			case 11:
				motion = "Spin";
				break;
			default:
				motion = "Shift";
				break;
			}
			launcher.animator.SetTrigger (motion);
		}
	}

	public void ShiftLaunchers ()
	{
		MoveLaunchers ();
		ChangeLaunchersTargets ();
		UpdateLauncherPositions ();

		float animationDuration = 1f / launcherAnimatorSpeed;
		StartCoroutine (OnShiftLaunchersEnd (animationDuration));
	}

	IEnumerator OnShiftLaunchersEnd (float time)
	{
		yield return new WaitForSeconds (time);
		yield return new WaitUntil (() => {
			foreach (Launcher launcher in launchers) {
				if (launcher.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime <= 1f) {
					return false;
				}
			}
			return true;
		});
		OnShiftEnd ();
		UpdateHints ();
		UpdateLauncherHolders ();
		ClickManager.instance.enabled = true;
	}

	private void UpdateHints ()
	{
		foreach (Launcher launcher in launchers) {
			launcher.ShowHints ();
		}
	}

	private void UpdateLauncherHolders ()
	{
		foreach (Launcher launcher in launchers) {
			launcher.animator.SetTrigger ("Idle");
			launcher.transform.SetParent (launcherHolders [launcher.position]);
		}
	}

	private void UpdateLauncherPositions ()
	{
		foreach (Launcher launcher in launchers) {
			launcher.position = (launcher.position + 1) % 12;
		}
	}
}
