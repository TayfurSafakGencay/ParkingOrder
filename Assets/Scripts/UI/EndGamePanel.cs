using System.Threading.Tasks;
using DG.Tweening;
using Enum;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
  public class EndGamePanel : MonoBehaviour
  {
    [Header("Stats")]
    [SerializeField]
    private GameObject _statsPart;

    [Space(10)]
    [SerializeField]
    private TextMeshProUGUI _coinText;

    [SerializeField]
    private TextMeshProUGUI _diamondText;

    [SerializeField]
    private TextMeshProUGUI _energyText;
    
    [Header("Win")]
    [SerializeField]
    private GameObject _winPart;

    [SerializeField]
    private TextMeshProUGUI _winPartLevelText;

    [Header("Fail")]
    [SerializeField]
    private GameObject _failPart;

    [SerializeField]
    private TextMeshProUGUI _failPartLevelText;
    
    private void Awake()
    {
      GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateKey gameStateKey)
    {
      switch (gameStateKey)
      {
        case GameStateKey.EndGameSuccess:
          OnGameFinished(true);
          break;
        case GameStateKey.EndGameFail:
          OnGameFinished(false);
          break;
        case GameStateKey.InGame:
          ClosePanel();
          break;
      }
    }

    private int _levelForEndGamePanel;

    public async void OnGameFinished(bool success)
    {
      _levelForEndGamePanel = LevelManager.Level;
      
      await Task.Delay(SpecialTimeKey.CarDestroyForSuccessfulPanelWaitingTime);

      SetStatsPart();
      
      CloseBothPanel();
      gameObject.SetActive(true);
      _statsPart.SetActive(true);
      
      if (success)
        OpenSuccessPanel();
      else
        OpenFailPart();
      
      PanelManager.Instance.PanelSlidingAnimationHorizontal(transform, Screen.width, 0);
    }

    private void SetStatsPart()
    {
      PlayerDataVo playerDataVo = DataManager.Instance.PlayerDataVo;
      
      _coinText.text = playerDataVo.Coin.ToString();
      _diamondText.text = playerDataVo.Diamond.ToString();
      _energyText.text = playerDataVo.Energy.ToString();
    }

    private void OpenSuccessPanel()
    {
      _winPart.SetActive(true);

      _winPartLevelText.text = "Level " + _levelForEndGamePanel;
    }

    private void OpenFailPart()
    {
      _failPart.SetActive(true);

      _failPartLevelText.text = "Level " + _levelForEndGamePanel;
    }
    
    public void OnPlay()
    {
      GameManager.Instance.GameStarted();
    }

    public void OnOpenShop()
    {
      // TODO: Safak
    }

    private void ClosePanel()
    {
      PanelManager.Instance.PanelSlidingAnimationHorizontal(transform, 0, Screen.width);
    }

    private void Animation(GameObject part)
    {
      part.transform.localScale = new Vector3(0, 0, 0);
      part.SetActive(true);
      part.transform.DOScale(new Vector3(1, 1, 1), 0.25f).SetEase(Ease.InQuart);
    }

    private void CloseBothPanel()
    {
      _winPart.SetActive(false);
      _failPart.SetActive(false);
      _statsPart.SetActive(false);
    }
  }
}