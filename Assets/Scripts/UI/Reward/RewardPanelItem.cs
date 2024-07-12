using UnityEngine;

namespace UI.Reward
{
  public class RewardPanelItem : MonoBehaviour
  {
    [SerializeField]
    private RewardKey _rewardKey;

    [Range(0, 3)]
    [SerializeField]
    private int _index;

    public int GetIndex => _index;

    public RewardKey GetRewardKey => _rewardKey;
  }
}