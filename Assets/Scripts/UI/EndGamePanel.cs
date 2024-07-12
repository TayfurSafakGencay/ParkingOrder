using DG.Tweening;
using Enum;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
  public class EndGamePanel : MonoBehaviour
  {
    [SerializeField]
    private GameObject _statsPart;
    
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
      if (gameStateKey == GameStateKey.EndGameSuccess) OnGameFinished(true);
      else if (gameStateKey == GameStateKey.EndGameFail) OnGameFinished(false);
    }

    public void NextButton()
    {
      // TODO: Safak
      // Main menuyu ac ve saga kaydir
      // Bu paneli saga kaydir sonra kapa
    }

    public void OnGameFinished(bool success)
    {
      CloseBothPanel();
      gameObject.SetActive(true);
      _statsPart.SetActive(true);
      
      if (success)
        OpenSuccessPanel();
      else
        OpenFailPart();
      
      PanelManager.Instance.PanelSlidingAnimationHorizontal(transform, Screen.width, 0);
    }

    private void OpenSuccessPanel()
    {
      _winPart.SetActive(true);

      _winPartLevelText.text = "Level " + LevelManager.Level;
    }

    private void OpenFailPart()
    {
      _failPart.SetActive(true);

      _failPartLevelText.text = "Level " + LevelManager.Level;
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