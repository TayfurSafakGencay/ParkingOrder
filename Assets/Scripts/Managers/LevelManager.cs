using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Enum;
using UnityEngine;

namespace Managers
{
  public class LevelManager : MonoBehaviour
  {
    public static LevelManager Instance;
    
    private Dictionary<int, LevelVo> _levelsData = new();

    public static LevelVo LevelData;

    public static int Level = 1;

    public static int CarCount { get; private set; }
    
    public int CompletedCars { get; private set; }

    public int RemainingMoves { get; private set; }
    
    private void Awake()
    {
      if (Instance == null) Instance = this;
      
      LoadLevelData();

      GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateKey gameState)
    {
      if (gameState == GameStateKey.EndGameSuccess)
      {
        OnGameFinishedSuccessfully();
      }
      else if (gameState == GameStateKey.InGame)
      {
        OnGameStarted();
      }
    }

    private void OnGameFinishedSuccessfully()
    {
      Level++;
    }

    private async void OnGameStarted()
    {
      CompletedCars = 0;
      CarCount = 0;

      LevelData = _levelsData[Level];
      
      RemainingMoves = LevelData.MovesCount;
    }

    public void ReachedPoint(bool end)
    {
      if (end) CompletedCars++;
      else CompletedCars--;

      if (CompletedCars == CarCount)
        GameManager.Instance.GameFinished(true);

      if (RemainingMoves <= 0)
        StartCoroutine(WaitMovingCars());
    }

    private IEnumerator WaitMovingCars()
    {
      while (CarManager.Instance.GetMovingCarCount() > 0)
        yield return null;
      
      GameManager.Instance.GameFinished(false);
    }

    public void Move()
    {
      if (RemainingMoves <= 0) return;
      
      RemainingMoves--;
      
      PanelManager.Instance.InGamePanel.OnMove(RemainingMoves);
    }

    public void IncreaseCarCount()
    {
      CarCount++;
    }

    private const string _dataPath = "Data/Level/Level Data";
    
    private void LoadLevelData()
    {
      LevelData data = Resources.Load<LevelData>(_dataPath);
      List<LevelVo> levelData = data.LevelVos;
      
      for (int i = 0; i < levelData.Count; i++)
      {
        _levelsData.Add(levelData[i].Level, levelData[i]);
      }
    }
  }
}