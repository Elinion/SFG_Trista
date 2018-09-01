using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

public class GameProgressWriter {
    public GameProgressWriter(string gameProgressFileName, Action<GameProgress.Game> onSaved) {
        this.gameProgressFileName = gameProgressFileName;
        this.onSaved = onSaved;
    }

    private readonly string gameProgressFileName;
    private GameProgress.Game gameProgress;
    private readonly Action<GameProgress.Game> onSaved;

    private ISavedGameClient SavedGame {
        get { return ((PlayGamesPlatform) Social.Active).SavedGame; }
    }

    public void save(GameProgress.Game gameProgressToSave) {
        gameProgress = gameProgressToSave;
        
        Social.localUser.Authenticate(isAuthenticated => {
            if (isAuthenticated) {
                saveGameProgressToCloud();
            } else {
                saveGameProgressToDisk();
            }
        });
    }

    private void saveGameProgressToCloud() {
        Debug.Log("Saving game progress to cloud");
        SavedGame.OpenWithAutomaticConflictResolution(
            gameProgressFileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseMostRecentlySaved,
            onGameProgressOpened);
    }

    private void saveGameProgressToDisk() {
        Debug.Log("Save game progress to disk");
    }

    private void onGameProgressOpened(SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status != SavedGameRequestStatus.Success) {
            Debug.LogWarning("Failed to open game progress. Status: " + status);
            return;
        }

        SavedGameMetadataUpdate savedGameMetadataUpdate = new SavedGameMetadataUpdate.Builder()
            .WithUpdatedDescription("Entering juicier pack!")
            .Build();

        SavedGame.CommitUpdate(
            game,
            savedGameMetadataUpdate,
            gameProgress.toSaveFormat(),
            onGameProgressSaved);
    }

    private void onGameProgressSaved(SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status != SavedGameRequestStatus.Success) {
            Debug.LogWarning("Failed to save game progress. Status: " + status);
            onSaved(null);
            return;
        }
        onSaved(gameProgress);
    }
}