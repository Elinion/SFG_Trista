using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    private LevelData level;
    public LevelData Level
    {
        
        get { return level; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //levelClearUI.SetActive (false);
    }

    public void GameOver()
    {
        //gameOverUI.SetActive (true);
    }

    public void GoToLevelMenu()
    {
        SceneManager.LoadSceneAsync("levelSelection");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync("mainMenu");
    }

    public void GoToNextLevel()
    {
        GlobalLevelData.selectedLevelId++;
        GlobalLevelData.selectedLevelId %= GlobalLevelData.numberOfLevels;
        ReloadScene();
    }

    public void PlayLevel(LevelData level)
    {
        this.level = level;
        SceneManager.LoadSceneAsync("game");
    }

    public void ReloadScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
