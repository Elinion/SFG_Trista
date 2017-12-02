using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPicker : MonoBehaviour
{
		public void OpenLevel (int levelId)
	{
		GlobalLevelData.selectedLevelId = levelId;
		Debug.Log (GlobalLevelData.selectedLevelId);
		SceneManager.LoadSceneAsync ("game");
	}

	public void GoToMainMenu ()
	{
		SceneManager.LoadSceneAsync ("mainMenu");
	}
}
