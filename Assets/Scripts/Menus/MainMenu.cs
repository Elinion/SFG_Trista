using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	private void Awake() {
		SceneManager.LoadScene(Tags.Persistent, LoadSceneMode.Additive);
	}

	public void quit ()
	{
		Application.Quit ();
	}

	public void goToLevels ()
	{
		GameController.instance.goToLevelSelectionMenu();
	}

	public void goToSurvivalMode ()
	{
		SceneManager.LoadSceneAsync ("survivalMode");
	}

	public void goToSettings() {
		LocalizationController.instance.deleteSavedLanguage();
		GameController.instance.goToLanguageSelection();
	}
}
