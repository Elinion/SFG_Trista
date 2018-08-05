using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	private void Awake() {
		SceneManager.LoadScene(Tags.Persistent, LoadSceneMode.Additive);
	}

	public void Quit ()
	{
		Application.Quit ();
	}

	public void GoToLevels ()
	{
		GameController.instance.GoToLevelMenu();
	}

	public void GoToSurvivalMode ()
	{
		SceneManager.LoadSceneAsync ("survivalMode");
	}
}
