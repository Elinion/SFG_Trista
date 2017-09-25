using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static GameController instance = null;
	public GameObject levelClearUI;
	public GameObject gameOverUI;

	private Board board;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}

		levelClearUI.SetActive (false);
		board = GameObject.FindGameObjectWithTag (Tags.Board).GetComponent<Board> ();
	}

	public void ClearLevel ()
	{
		levelClearUI.SetActive (true);
	}

	public void GameOver ()
	{
		gameOverUI.SetActive (true);
	}

	public void Restart ()
	{
		board.UnsubscribeFromEvents ();
		SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().name);
	}
}
