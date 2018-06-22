using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    public LevelData level { get; private set; }

    private void Awake()
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
        reloadScene();
    }

    public void playLevel(LevelData level)
    {
        this.level = level;
        SceneManager.LoadSceneAsync("game");
    }

    public void reloadScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
