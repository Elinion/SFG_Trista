using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static GameController instance = null;
	public GameObject levelClearUI;
	public GameObject gameOverUI;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}

		levelClearUI.SetActive (false);
	}

	public void ClearLevel ()
	{
		levelClearUI.SetActive (true);
	}

	public void GameOver ()
	{
		gameOverUI.SetActive (true);
	}

	public void GoToLevelMenu ()
	{
		SceneManager.LoadSceneAsync ("levelMenu");
	}

	public void GoToMainMenu ()
	{
		SceneManager.LoadSceneAsync ("mainMenu");
	}

	public void GoToNextLevel ()
	{
		GlobalLevelData.selectedLevelId++;
		GlobalLevelData.selectedLevelId %= GlobalLevelData.numberOfLevels;
		Restart ();
	}

	public void Restart ()
	{
		SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().name);
	}
}
