using System;
using Enum;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

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

    private void Start()
    {
      GameStarted();
    }

    public void GameStarted()
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