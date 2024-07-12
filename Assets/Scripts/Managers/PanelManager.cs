using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Enum;
using UI;
using UnityEngine;

namespace Managers
{
  public class PanelManager : MonoBehaviour
  {
    public static PanelManager Instance;
    
    public InGamePanel InGamePanel;

    public EndGamePanel EndGamePanel;

    private void Awake()
    {
      if (Instance == null) Instance = this;

      ChangeAllPanelsSetActive(true);
    }

    private void Start()
    {
      ChangeAllPanelsSetActive(false);
    }
    
    private void ChangeAllPanelsSetActive(bool open)
    {
      InGamePanel.gameObject.SetActive(open);
      EndGamePanel.gameObject.SetActive(open);
    }

    public TweenerCore<Vector3, Vector3, VectorOptions> PanelSlidingAnimationHorizontal(Transform panel, float initialXPosition, float lastXPosition, float time = SpecialTimeKey.PanelSliding)
    {
      panel.localPosition = new Vector3(initialXPosition, 0, 0);
      return panel.DOLocalMoveX(lastXPosition, time).SetEase(Ease.OutBack);
    }
  }
}