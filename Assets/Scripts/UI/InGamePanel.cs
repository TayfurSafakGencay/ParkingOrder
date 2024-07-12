using System.Threading.Tasks;
using Data;
using DG.Tweening;
using Enum;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
  public class InGamePanel : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI _timerText;

    [SerializeField]
    private TextMeshProUGUI _movesText;

    private float _timer;

    private void Awake()
    {
      GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateKey gameStateKey)
    {
      if (gameStateKey == GameStateKey.InGame)
      {
        OpenPanel();
      }
      else
      {
        ClosePanel();
      }
    }

    private async void OpenPanel()
    {
      await Task.Delay(SpecialTimeKey.WaitLevelData);
      
      LevelVo levelVo = LevelManager.LevelData;
      SetTimer(levelVo.Time);
      _movesText.text = levelVo.MovesCount.ToString();
      
      gameObject.SetActive(true);

      await PanelManager.Instance.PanelSlidingAnimationHorizontal(transform, -Screen.width, 0).AsyncWaitForCompletion();
      
      StartTimer();
    }

    private async void ClosePanel()
    {
      await Task.Delay(SpecialTimeKey.CarDestroyForSuccessfulPanelWaitingTime);
      
      await PanelManager.Instance.PanelSlidingAnimationHorizontal(transform, 0, -Screen.width).AsyncWaitForCompletion();

      gameObject.SetActive(false);
    }

    public void OnMove(int remainingMove)
    {
      _movesText.text = remainingMove.ToString();
    }
    
    private void SetTimer(int initialTime)
    {
      _timer = initialTime;
      SetTimerText();
    }

    private async void StartTimer()
    {
      while (_timer > 0)
      {
        SetTimerText();
        await Task.Delay(1000);
        _timer--;

        if (GameManager.GameStateKey != GameStateKey.InGame) return;
      }
      
      SetTimerText();
      GameManager.Instance.GameFinished(false);
    }

    private void SetTimerText()
    {
      _timerText.text = _timer.ToString("f0");
    }
  }
}