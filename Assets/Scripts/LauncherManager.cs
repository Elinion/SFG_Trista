using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherManager : MonoBehaviour
{
	public static LauncherManager instance = null;

	public delegate void OnLaunchersShiftEndAction ();

	public static event OnLaunchersShiftEndAction OnLaunchersShiftEnd;

	public List<Launcher> launchers = new List<Launcher> ();
	public List<Transform> launcherHolders = new List<Transform> ();

	private bool shouldUpdateLauncherHolders = false;
	private bool animationHasStarted = false;
	private Launcher triggeredLauncher = null;

	void Awake ()
	{
		ImplementSingleton ();
	}

	void Start ()
	{
		UpdateHints ();
		Board.OnRemoveTriplesEnd += UpdateLaunchers;
	}

	void Update ()
	{
		HandleLaunchersAnimation ();
	}

	public void TriggerLauncher (Launcher launcher)
	{
		bool didLaunch = launcher.GetComponent<Launcher> ().Trigger ();
		if (didLaunch) {
			triggeredLauncher = launcher;
			ClickManager.instance.enabled = false;
			HideHints ();

			// momemtarily managed by the triggered launcher
			// this will be move to a launcherHolder script
			// ShiftLaunchers ();
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
				OnLaunchersShiftEnd ();
			}
		}
	}

	private void HideHints ()
	{
		Board.instance.HideHints ();
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

	private void UpdateLaunchers ()
	{
		UpdateHints ();
		UpdateLauncherHolders ();
		triggeredLauncher.ChangeType ();
		ClickManager.instance.enabled = true;
	}
}
