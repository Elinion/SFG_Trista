using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
	public static ClickManager instance = null;
	private GameObject[] launchers;

	void Awake ()
	{
		ImplementSingleton ();
		launchers = GameObject.FindGameObjectsWithTag (Tags.Launcher);
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Vector2 mousePos2D = new Vector2 (mousePos.x, mousePos.y);

			RaycastHit2D hit = Physics2D.Raycast (mousePos2D, Vector2.zero);
			if (hit.collider != null) {
				TriggerLaunchers (hit.transform.gameObject);
			}
		}
	}

	private void TriggerLaunchers (GameObject clickedObject)
	{
		foreach (GameObject launcher in launchers) {
			if (launcher == clickedObject) {
				launcher.GetComponent<Launcher> ().Trigger ();
				TileManager.instance.RemoveTriples ();
				LauncherManager.instance.UpdateHints ();
			}
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
