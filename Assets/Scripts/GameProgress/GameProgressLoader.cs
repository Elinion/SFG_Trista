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
        get { return ((PlayGamesPlatform) Social.Active).SavedGame; }
    }

    public void load() {
        Social.localUser.Authenticate(isAuthenticated => {
            if (isAuthenticated) {
                loadGameProgressFromCloud();
            } else {
                loadGameProgressFromDisk();
            }
        });
    }

    private void loadGameProgressFromCloud() {
        Debug.Log("Loading game progress from cloud");
        SavedGame.OpenWithAutomaticConflictResolution(
            gameProgressFileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseMostRecentlySaved,
            onGameProgressOpened);
    }

    private void loadGameProgressFromDisk() {
        Debug.Log("Loading game progress from disk");
        GameProgress.Game loadedGameProgress = new GameProgress.Game();
        loadedGameProgress.init();
        onLoaded(loadedGameProgress);
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