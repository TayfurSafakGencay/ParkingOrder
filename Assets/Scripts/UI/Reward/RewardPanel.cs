using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Enum;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

    private readonly Dictionary<int, RewardKey> _rewards = new();

    private void Awake()
    {
      GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateKey gameState)
    {
      if (gameState == GameStateKey.Reward)
      {
        OpenPanel();
      }
      else
      {
        ClosePanel();
      }
    }

    private void OpenPanel()
    {
      gameObject.SetActive(true);
      _stopCandleAnimationButton.interactable = false;
      _buttonRewardImage.DOFade(0, 0);
      
      SetMethods();
      SetRewards();
      
      _candle.transform.localRotation = Quaternion.Euler(0, 0, 90);

      PanelManager.Instance.PanelSlidingAnimationHorizontal(transform, Screen.width, 0).OnComplete(() =>
      {
        _buttonRewardImage.DOFade(1, 0.25f);

        CandleAnimation();
        _stopCandleAnimationButton.interactable = true;
      });
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
      _rewards.Clear();
      
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
      _candleAnimation = _candle.transform.DOLocalRotate(new Vector3(0, 0, -90), 1f)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Yoyo).OnUpdate(ButtonImageChanger);
    }

    [Space(10)]
    [SerializeField]
    private Image _buttonRewardImage;

    [SerializeField]
    private Button _stopCandleAnimationButton;
    private void ButtonImageChanger()
    {
      Vector3 rotation = _candle.transform.localRotation.eulerAngles;
      float zRot = rotation.z;
      if (zRot > 180) zRot -= 360;
      
      Sprite sprite = zRot switch
      {
        >= 36 => _chestSprite,
        < 36 and >= -18 => _coinSprite,
        < -18 and >= -58.5f => _energySprite,
        _ => _diamondSprite
      };

      _buttonRewardImage.sprite = sprite;
    }

    public async void OnStopCandleAnimation()
    {
      _stopCandleAnimationButton.interactable = false;
      _candleAnimation.Kill();

      await Task.Delay(SpecialTimeKey.WaitStopCandle);

      Vector3 rotation = _candle.transform.localRotation.eulerAngles;
      float zRot = rotation.z;
      if (zRot > 180) zRot -= 360;

      RewardKey rewardKey = zRot switch
      {
        >= 36 => _rewards[0],
        < 36 and >= -18 => _rewards[1],
        < -18 and >= -58.5f => _rewards[2],
        _ => _rewards[3]
      };

      ExecuteReward(rewardKey);

      await Task.Delay((int)(_animationTime * 1000) * 2);
      PanelManager.Instance.PanelSlidingAnimationHorizontal(transform, 0, Screen.width).OnComplete(() =>
      {
        gameObject.SetActive(false);
      });
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

    [Space(10)]
    [SerializeField]
    private Sprite _chestSprite;
    private void Chest()
    {
      GameManager.ChangeGameState(GameStateKey.ChestOpening);
    }

    [Space(10)]
    [SerializeField]
    private Sprite _coinSprite;

    [SerializeField]
    private Transform _coinTarget;

    private void Coin()
    {
      PlayAnimation(_coinTarget, PanelManager.Instance.EndGamePanel.CoinText, 10, _coinSprite, Random.Range(50, 200), PlayerPrefKey.Coin);
    }

    [Space(10)]
    [SerializeField]
    private Sprite _energySprite;

    [SerializeField]
    private Transform _energyTarget;

    private void Energy()
    {
      int randomEnergy = Random.Range(5, 15);
      PlayAnimation(_energyTarget, PanelManager.Instance.EndGamePanel.EnergyText, randomEnergy, _energySprite, randomEnergy, PlayerPrefKey.Energy);
    }

    [Space(10)]
    [SerializeField]
    private Sprite _diamondSprite;

    [SerializeField]
    private Transform _diamondTarget;

    private void Diamond()
    {
      PlayAnimation(_diamondTarget, PanelManager.Instance.EndGamePanel.DiamondText, 10, _diamondSprite, Random.Range(10, 25), PlayerPrefKey.Diamond);
    }

    [Space(20)]
    [SerializeField]
    private GameObject _animationObject;

    private const float _animationTime = 1.5f;

    private async void PlayAnimation(Transform target, TextMeshProUGUI text, int itemAmount, Sprite animationObjectSprite, int amount, PlayerPrefKey playerPrefKey)
    {
      for (int i = 0; i < itemAmount; i++)
      {
        Vector3 position = new(Random.Range(-200, 200), Random.Range(-200, 200), 0);

        GameObject item = Instantiate(_animationObject, Vector3.zero, Quaternion.identity, transform);
        item.GetComponent<Image>().sprite = animationObjectSprite;
        item.transform.localPosition = position;

        if (i == 0)
        {
          item.transform.DOMove(target.position, _animationTime).SetEase(Ease.InBack).OnComplete(() =>
          {
            UpdateText(text, playerPrefKey, amount);
            GrowAndShrinkAnimation(target);
            Destroy(item);
          });
        }
        else if (i + 1 == itemAmount)
        {
          await item.transform.DOMove(target.position, _animationTime).SetEase(Ease.InBack).OnComplete(() =>
          {
            GrowAndShrinkAnimation(target);
            Destroy(item);
          }).AsyncWaitForCompletion();
        }
        else
        {
          item.transform.DOMove(target.position, _animationTime).SetEase(Ease.InBack).OnComplete(() =>
          {
            GrowAndShrinkAnimation(target);
            Destroy(item);
          });
        }

        await Task.Delay(100);
      }
    }

    private const float _endScale = 1.25f;

    private const float _initialAnimationTime = 0.1f;

    private void GrowAndShrinkAnimation(Transform target)
    {
      target.DOScale(_endScale, _initialAnimationTime)
        .OnComplete(() => { target.DOScale(1, _initialAnimationTime); });
    }

    private void UpdateText(TextMeshProUGUI counterText, PlayerPrefKey playerPrefKey, int amount)
    {
      int data = DataManager.Instance.LoadInt(playerPrefKey);
      int textData = data;

      DOTween.To(() => textData, x => textData = x, amount + data, _animationTime).OnUpdate(() => { counterText.text = textData.ToString("f0"); });

      DataManager.Instance.SaveInt(playerPrefKey, amount + data);
    }

    private void ClosePanel()
    {
      gameObject.SetActive(false);
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