using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sequence = Unity.VisualScripting.Sequence;

namespace UI.Shop
{
  public class ShopPanelItem : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI _diamondPriceText;

    [SerializeField]
    private TextMeshProUGUI _coinPriceText;

    [SerializeField]
    private TextMeshProUGUI _titleText;

    [SerializeField]
    private Transform _diamondImage;

    [SerializeField]
    private Button _buyButton;

    private GameObject _carObject;

    private int _coinPrice;

    private int _diamondReward;

    private bool _owned;

    private int _id;

    public void SetData(GameObject carObject, int coinPrice, int diamondPrice, bool owned, int id)
    {
      _carObject = carObject;

      _coinPrice = coinPrice;
      _diamondReward = diamondPrice;

      _owned = owned;
      _id = id;
      
      SetUI();
      CreateObject();
    }

    private void SetUI()
    {
      if (_owned)
      {
        _buyButton.gameObject.SetActive(false);
        _diamondPriceText.gameObject.SetActive(false);
        _diamondImage.gameObject.SetActive(false);
      }
      
      int coin = DataManager.Instance.LoadInt(PlayerPrefKey.Coin);

      if (coin > _coinPrice)
      {
        _coinPriceText.text = $"<color=white>{_coinPrice}</color>";
        _buyButton.interactable = true;
      }
      else
      {
        _coinPriceText.text = $"<color=red>{_coinPrice}</color>";
        _buyButton.interactable = false;
      }

      _diamondPriceText.text = _diamondReward.ToString();
      
      _titleText.text = "Model " + (_id + 1);
    }

    private const float _rotationAnimationTime = 4f;

    private GameObject _car3D;

    private void CreateObject()
    {
      _car3D = Instantiate(_carObject, Vector3.zero, Quaternion.identity, transform);

      _car3D.transform.localPosition = new Vector3(0, 15, -200);
      _car3D.transform.localScale = new Vector3(125, 125, 125);
      
      _car3D.transform.DORotate(new Vector3(0, 360, 0), _rotationAnimationTime, RotateMode.FastBeyond360)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Incremental);
    }

    public void BuyProduct()
    {
      PanelManager.Instance.ShopPanel.BuyCar(_id, _diamondReward, _coinPrice);
      
      _buyButton.interactable = false;
      _owned = true;
      
      SetUI();

      DG.Tweening.Sequence carBuySequence = DOTween.Sequence();

      carBuySequence.Append(_car3D.transform.DOScale(225, 1f).SetEase(Ease.InCubic));
      carBuySequence.AppendInterval(2f);
      carBuySequence.Append(_car3D.transform.DOScale(150, 1f).SetEase(Ease.InCubic));
    }

    private void OnDisable()
    {
      _car3D.transform.DOKill();
    }
  }
}