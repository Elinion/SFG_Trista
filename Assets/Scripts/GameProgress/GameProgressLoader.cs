using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

public class GameProgressLoader {
    public GameProgressLoader(string gameProgressFileName, Action<GameProgress.Game> onLoaded) {
        this.gameProgressFileName = gameProgressFileName;
        this.onLoaded = onLoaded;
    }

    private readonly string gameProgressFileName;
    private readonly Action<GameProgress.Game> onLoaded;

    private ISavedGameClient SavedGame {
#if UNITY_ANDROID
        get { return ((PlayGamesPlatform) Social.Active).SavedGame; }
#else
        get { return null; }
#endif
    }

    public void load() {
//        Social.localUser.Authenticate(isAuthenticated => {
//            if (isAuthenticated) {
//                loadGameProgressFromCloud();
//            } else {
//                loadGameProgressFromDisk();
//            }
//        });
        loadGameProgressFromDisk();
    }

    private void loadGameProgressFromCloud() {
        Debug.Log("Loading game progress from cloud");
//        SavedGame.OpenWithAutomaticConflictResolution(
//            gameProgressFileName,
//            DataSource.ReadCacheOrNetwork,
//            ConflictResolutionStrategy.UseMostRecentlySaved,
//            onGameProgressOpened);
    }

    private void loadGameProgressFromDisk() {
        Debug.Log("Loading game progress from disk");
        GameProgress.Game gameProgress = Files.readFromJson<GameProgress.Game>(gameProgressFileName) ?? new GameProgress.Game();
        onLoaded(gameProgress);
    }

    private void onGameProgressOpened(SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status != SavedGameRequestStatus.Success) {
            Debug.LogWarning("Failed to open game progress. Status: " + status);
            return;
        }

        SavedGame.ReadBinaryData(game, onGameProgressLoaded);
    }

    private void onGameProgressLoaded(SavedGameRequestStatus status, byte[] gameProgressBytes) {
        if (status != SavedGameRequestStatus.Success) {
            Debug.LogWarning("Failed to read game progress. Status: " + status);
            return;
        }

        GameProgress.Game loadedGameProgress = GameProgress.Game.fromSaveFormat(gameProgressBytes);
        onLoaded(loadedGameProgress);
    }
}