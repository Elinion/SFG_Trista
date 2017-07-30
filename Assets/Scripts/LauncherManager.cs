using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherManager : MonoBehaviour
{
	public List<Launcher> launchers = new List<Launcher> ();
	public List<Transform> launcherHolders = new List<Transform> ();

	private bool shouldUpdateLauncherHolders = false;
	private bool animationHasStarted = false;
	private Launcher triggeredLauncher = null;

	void Start ()
	{
		UpdateHints ();
	}

	void Update ()
	{
		HandleLaunchersAnimation ();
	}

	public void TriggerLauncher (Launcher launcher)
	{
		if (launcher.GetComponent<Launcher> ().Trigger ()) {
			triggeredLauncher = launcher;
			HideHints ();
			ShiftLaunchers ();
		}
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

	private void HandleLaunchersAnimation ()
	{
		if (shouldUpdateLauncherHolders) {
			bool readyToUpdateLaunchers = !launchers [0].animator.GetCurrentAnimatorStateInfo (0).IsName ("Idle");
			if (readyToUpdateLaunchers) {
				shouldUpdateLauncherHolders = false;
				animationHasStarted = true;
			}
		} else if (animationHasStarted) {
			bool animationIsFinished = launchers [0].animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1f;
			if (animationIsFinished) {
				animationHasStarted = false;
				TileManager.instance.RemoveTriples ();
				UpdateHints ();
				UpdateLauncherHolders ();
				triggeredLauncher.ChangeType ();
			}
		}
	}

	private void HideHints ()
	{
		foreach (Launcher launcher in launchers) {
			launcher.HideHints ();
		}
		TileManager.instance.HideHints ();
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

	private void ShiftLaunchers ()
	{
		MoveLaunchers ();
		ChangeLaunchersTargets ();
		UpdateLauncherPositions ();
		shouldUpdateLauncherHolders = true;
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
