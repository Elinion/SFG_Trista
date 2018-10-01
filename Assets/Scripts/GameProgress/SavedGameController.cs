using GameProgress;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using GameElement = GameData.GameElement;
using Level = GameData.Level;
using LevelGroup = GameData.LevelGroup;

public class SavedGameController : MonoBehaviour {
    public static SavedGameController instance;

    private Game gameProgress { get; set; }

    private const string GameProgressFileName = "gameProgress.json";
    private GameProgressSynchronizer gameProgressSynchronizer;
    private GameProgressLoader gameProgressLoader;
    private GameProgressWriter gameProgressWriter;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        gameProgressSynchronizer = new GameProgressSynchronizer();
        gameProgressLoader = new GameProgressLoader(GameProgressFileName, onGameProgressLoaded);
        gameProgressWriter = new GameProgressWriter(GameProgressFileName, onGameProgressSaved);
    }

    public void loadGameProgress() {
//        configurePlayGamesClient();
        gameProgressLoader.load();
    }

    public State getLevelState(Level level) {
        if (!gameProgress.levelsById.ContainsKey(level.id)) {
            logGameElementNotFound(level);
            return State.NotOk;
        }

        return gameProgress.levelsById[level.id].state;
    }

    public State getLevelGroupState(LevelGroup levelGroup) {
        if (!gameProgress.levelGroupsById.ContainsKey(levelGroup.id)) {
            logGameElementNotFound(levelGroup);
        }

        return gameProgress.levelGroupsById[levelGroup.id].state;
    }

    private void onGameProgressLoaded(Game loadedGameProgress) {
        gameProgress = loadedGameProgress;
        gameProgressSynchronizer.synchronizeGameDataAndProgress(loadedGameProgress);
    }

    private void onGameProgressSaved(Game savedGameProgress) {
        if (savedGameProgress == null) {
            Debug.Log("Could not save game progress: the game progress object was null.");
        }

        // todo add saving game spinner
    }

    private void logGameElementNotFound(GameElement gameElement) {
        Debug.Log(string.Format("Could not get {0} state. Id not found. Name: {1}. Id: {2}",
            gameElement.GetType(),
            gameElement.name,
            gameElement.id));
    }

    public void updateLevelState(Level level, State state) {
        if (gameProgress.levelsById.ContainsKey(level.id)) {
            gameProgress.levelsById[level.id].state = state;
            saveGameProgress();
        } else {
            Debug.Log("Could not save level progress. Id not found: " + level.id);
        }
    }

    private void saveGameProgress() {
        // todo prevent ui to show after each level
//        gameProgress.score = Random.Range(0, 100);
        gameProgress.toSaveFormat();
        gameProgressWriter.save(gameProgress);
    }

    public bool isGameProgressLoaded() {
        return gameProgress != null;
    }

    private void configurePlayGamesClient() {
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
            .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
#endif
    }

    public bool isUserAuthenticated() {
        return Social.Active.localUser.authenticated;
    }
}