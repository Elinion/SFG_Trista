using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	public void Quit ()
	{
		Application.Quit ();
	}

	public void GoToLevels ()
	{
		SceneManager.LoadSceneAsync ("levelMenu");
	}

	public void GoToSurvivalMode ()
	{
		GlobalLevelData.selectedLevelId = -1;
		SceneManager.LoadSceneAsync ("survivalMode");
	}
}
