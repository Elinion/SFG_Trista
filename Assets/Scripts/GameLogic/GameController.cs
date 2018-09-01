using GameData;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public static GameController instance;

    public Level level { get; private set; }

    private int selectedWorld;
    private int selectedLevelGroup;
    private int selectedLevel;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void goToLanguageSelection() {
        SceneManager.LoadScene(Scenes.LanguageSelectionMenu);
    }

    public void goToLevelSelectionMenu() {
        SceneManager.LoadScene(Scenes.LevelSelectionMenu);
    }

    public void goToMainMenu() {
        SceneManager.LoadScene(Scenes.MainMenu);
    }

    public void goToNextLevel() {
        int nextLevelId = selectedLevel + 1;
        playLevel(getLevelGroup().levels[nextLevelId]);
    }

    public void selectNextLevelGroup() {
        selectedLevelGroup++;
    }

    public void selectPreviousLevelGroup() {
        selectedLevelGroup = Mathf.Max(0, selectedLevelGroup - 1);
    }

    public LevelGroup getNextLevelGroup() {
        return ByOrderIndexAccessor.getLevelGroup(selectedWorld, selectedLevelGroup + 1);
    }

    public bool isNextLevel() {
        return ByOrderIndexAccessor.getLevel(selectedWorld, selectedLevelGroup, selectedLevel + 1) != null;
    }

    public LevelGroup getLevelGroup() {
        return ByOrderIndexAccessor.getLevelGroup(selectedWorld, selectedLevelGroup);
    }

    public bool isNextLevelGroup() {
        return ByOrderIndexAccessor.getLevelGroup(selectedWorld, selectedLevelGroup + 1) != null;
    }

    public void playLevel(Level level) {
        setLevel(level);
        SceneManager.LoadScene(Scenes.Game);

        if (!string.IsNullOrEmpty(level.sceneAdditiveName)) {
            SceneManager.LoadScene(level.sceneAdditiveName, LoadSceneMode.Additive);
        }
    }

    private void setLevel(Level level) {
        this.level = level;
        selectedLevel = level.orderIndex;
    }


    public void reloadScene() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}