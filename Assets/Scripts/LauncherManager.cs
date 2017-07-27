using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherManager : MonoBehaviour
{
	public static LauncherManager instance;

	private List<Launcher> launchers = new List<Launcher> ();

	void Awake ()
	{
		ImplementSingleton ();
		FetchAllLauncherScripts ();
	}

	void Start ()
	{
		UpdateHints ();
	}

	public void UpdateHints ()
	{
		foreach (Launcher launcher in launchers) {
			launcher.ShowHints ();
		}
	}

	private void FetchAllLauncherScripts ()
	{
		GameObject[] launcherGameObjects = GameObject.FindGameObjectsWithTag (Tags.Launcher);
		foreach (GameObject launcherGameObject in launcherGameObjects) {
			launchers.Add (launcherGameObject.GetComponent<Launcher> ());
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
}
