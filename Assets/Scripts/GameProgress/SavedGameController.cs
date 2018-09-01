using GameProgress;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using Level = GameData.Level;

public class SavedGameController : MonoBehaviour {
    public static SavedGameController instance;

    public Game gameProgress { get; private set; }

    private string gameProgressFileName = "gameProgress";
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
        gameProgressLoader = new GameProgressLoader(gameProgressFileName, onGameProgressLoaded);
        gameProgressWriter = new GameProgressWriter(gameProgressFileName, onGameProgressSaved);
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

    public void loadGameProgress() {
        configurePlayGamesClient();
        gameProgressLoader.load();
    }

    public State getLevelState(Level level) {
        return gameProgressSynchronizer.getLevelState(level.id);
    }

    public void updateLevelState(Level level, State state) {
        if (gameProgress.levels.ContainsKey(level.id)) {
            gameProgress.levels[level.id].state = state;
            saveGameProgress();
        } else {
            Debug.Log("Could not save level progress. Id not found: " + level.id);
        }
    }

    private void saveGameProgress() {
        gameProgress.score = Random.Range(0, 100);
        gameProgressWriter.save(gameProgress);
    }

    public bool isGameProgressLoaded() {
        return gameProgress != null;
    }

    private void configurePlayGamesClient() {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
            .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
    }

    public bool isUserAuthenticated() {
        return Social.Active.localUser.authenticated;
    }
}