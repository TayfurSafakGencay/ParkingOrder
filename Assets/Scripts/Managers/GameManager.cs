using System;
using UnityEngine;

namespace Managers
{
  public class GameManager : MonoBehaviour
  {
    public static GameManager Instance;

    private void Awake()
    {
      if (Instance == null) Instance = this;
    }

    private void Start()
    {
      GameStarted();
    }

    public static Action OnGameStarted;
    
    public void GameStarted()
    {
      OnGameStarted?.Invoke();  
    }

    public static Action<bool> OnGameFinished;

    public void GameFinished(bool success)
    {
      OnGameFinished?.Invoke(success);
    }
  }
}