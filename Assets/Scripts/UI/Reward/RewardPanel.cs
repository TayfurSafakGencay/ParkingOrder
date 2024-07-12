using System;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace UI.Reward
{
  public class RewardPanel : MonoBehaviour
  {
    public delegate void RewardMethods();
    
    private readonly Dictionary<RewardKey, RewardMethods> _methods = new();

    [SerializeField]
    private GameObject _candle;

    [SerializeField]
    private Transform _itemParent;

    private Dictionary<int, RewardKey> _rewards = new();

    private void OnEnable()
    {
      SetMethods();
      CandleAnimation();
      SetRewards();
    }

    private void SetMethods()
    {
      _methods.Clear();
      
      RewardKey[] rewards = (RewardKey[])System.Enum.GetValues(typeof(RewardKey));
      for (int i = 0; i < rewards.Length; i++)
      {
        string methodName = rewards[i].ToString();
        MethodInfo method = GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

        if (method == null) continue;
        
        RewardMethods del = (RewardMethods)Delegate.CreateDelegate(typeof(RewardMethods), this, method);
        _methods.Add(rewards[i], del);
      }
    }

    private void SetRewards()
    {
      for (int i = 0; i < _itemParent.childCount; i++)
      {
        bool checkComponent = _itemParent.GetChild(i).TryGetComponent(out RewardPanelItem rewardPanelItem);

        if (!checkComponent) continue;
        
        _rewards.Add(rewardPanelItem.GetIndex, rewardPanelItem.GetRewardKey);
      }
    }

    private TweenerCore<Quaternion, Vector3, QuaternionOptions> _candleAnimation;
    private void CandleAnimation()
    {
      _candleAnimation = _candle.transform.DORotate(new Vector3(0, 0, 90), 1f)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Yoyo);
    }

    public void OnStopCandleAnimation()
    {
      _candleAnimation.Kill();

      RewardKey rewardKey;
      Vector3 rotation = _candle.transform.rotation.eulerAngles;
      float zRot = rotation.z;
      
      switch (zRot)
      {
        case >= 36:
          rewardKey = _rewards[0];
          break;
        case < 36 and >= -18:
          rewardKey = _rewards[1];
          break;
        default:
        {
          if (zRot < -18 && zRot >= 58.5f)
          {
            rewardKey = _rewards[2];
          }
          else
          {
            rewardKey = _rewards[3];
          }

          break;
        }
      }
      
      ExecuteReward(rewardKey);
    }
    
    public void ExecuteReward(RewardKey key)
    {
      if (_methods.TryGetValue(key, out RewardMethods method))
      {
        method.Invoke();
      }
      else
      {
        Console.WriteLine("Reward method not found");
      }
    }

    private void Chest()
    {
      print(nameof(Chest));
    }

    private void Coin()
    {
      print(nameof(Coin));
    }

    private void Energy()
    {
      print(nameof(Energy));
    }

    private void Diamond()
    {
      print(nameof(Diamond));
    }
  }

  public enum RewardKey
  {
    Chest,
    Coin,
    Energy,
    Diamond
  }
}