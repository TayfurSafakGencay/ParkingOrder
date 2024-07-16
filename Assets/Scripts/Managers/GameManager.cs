using System;
using Enum;
using UnityEngine;

namespace Managers
{
  public class GameManager : MonoBehaviour
  {
    public static GameManager Instance;

    public static GameStateKey GameStateKey = GameStateKey.Initial;

    public static Action<GameStateKey> OnGameStateChanged;

    private void Awake()
    {
      if (Instance == null) Instance = this;

      OnGameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameStateKey newGameStateKey)
    {
      GameStateKey = newGameStateKey;
    }

    public static void ChangeGameState(GameStateKey gameStateKey)
    {
      OnGameStateChanged?.Invoke(gameStateKey);
    }

    private void Start()
    {
      GameStart();
    }

    public void GameStart()
    {
      OnGameStateChanged?.Invoke(GameStateKey.InGame);
    }

    public void GameFinished(bool success)
    {
      if (GameStateKey != GameStateKey.InGame) return;
      
      OnGameStateChanged?.Invoke(success ? GameStateKey.EndGameSuccess : GameStateKey.EndGameFail);
    }
  }
}