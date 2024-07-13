using Enum;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  public class ChestOpeningPanel : MonoBehaviour
  {
    [SerializeField]
    private Image _background;
    
    
    private void Awake()
    {
      GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateKey gameState)
    {
      if (gameState != GameStateKey.ChestOpening) return;
      
      OpenPanel();
    }

    private void OpenPanel()
    {
      
    }
  }
}