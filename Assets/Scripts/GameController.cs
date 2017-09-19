using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static GameController instance = null;

	private Board board;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}

		board = GameObject.FindGameObjectWithTag (Tags.Board).GetComponent<Board> ();
	}

	public void GameOver ()
	{
		Debug.Log ("Game Over");
	}

	public void Restart ()
	{
		board.UnsubscribeFromEvents ();
		SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().name);
	}
}
