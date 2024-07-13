using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Enum;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers
{
  public class LevelManager : MonoBehaviour
  {
    [SerializeField]
    private Transform _levelPool;
    
    public static LevelManager Instance;
    
    private readonly Dictionary<int, LevelVo> _levelsData = new();

    public static LevelVo LevelData;

    public static int Level;

    public static int CarCount { get; private set; }
    
    public int CompletedCars { get; private set; }

    public int RemainingMoves { get; private set; }
    
    private async void Awake()
    {
      if (Instance == null) Instance = this;
      
      GameManager.OnGameStateChanged += OnGameStateChanged;

      await Task.Delay(SpecialTimeKey.WaitLevelData / 5);

      Level = DataManager.Instance.LoadInt(PlayerPrefKey.Level);
      LoadLevelData();
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

    private async void OnGameFinishedSuccessfully()
    {
      await Task.Delay(SpecialTimeKey.WaitLevelData);
      
      DataManager.Instance.SaveInt(PlayerPrefKey.Level, Level++);

      Level = DataManager.Instance.LoadInt(PlayerPrefKey.Level);
    }

    private async void OnGameStarted()
    {
      await Task.Delay(SpecialTimeKey.WaitLevelData / 2);
      
      CompletedCars = 0;
      CarCount = 0;

      LevelData = _levelsData[Level];
      
      RemainingMoves = LevelData.MovesCount;
      
      CreateLevel();
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
    
    private GameObject _levelObject;

    private async void CreateLevel()
    {
      if (_levelObject != null)
      {
        Destroy(_levelObject);
      }
      
      string levelName = "Level " + Level;
      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(levelName, Vector3.zero, Quaternion.identity, _levelPool);

      await asyncOperationHandle.Task;
      
      if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
      {
        _levelObject = asyncOperationHandle.Result;
      }
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