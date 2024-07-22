using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
  public class ShopPanel : MonoBehaviour
  {
    [Space(10)]
    [Header("Stats")]
    [SerializeField]
    private TextMeshProUGUI _coinText;

    [SerializeField]
    private Transform _coinImage;

    [Space(10)]
    [SerializeField]
    private TextMeshProUGUI _energyText;

    [SerializeField]
    private Transform _energyImage;

    [Space(10)]
    [SerializeField]
    private TextMeshProUGUI _diamondText;

    [SerializeField]
    private Transform _diamondImage;

    private void Awake()
    {
      gameObject.SetActive(false);
    }

    private void OnEnable()
    {
      OpenPanel();
    }

    public void ClosePanel()
    {
      gameObject.SetActive(false);

      DestroyItems();
    }
    
    private void DestroyItems()
    {
      for (int i = 0; i < _gameProductsPool.childCount; i++)
      {
        Destroy(_gameProductsPool.GetChild(i).gameObject);
      }

      for (int i = 0; i < _diamondProductsPool.childCount; i++)
      {
        Destroy(_diamondProductsPool.GetChild(i).gameObject);
      }
      
      for (int i = 0; i < _inGameProductsPool.childCount; i++)
      {
        Destroy(_inGameProductsPool.GetChild(i).gameObject);
      }
    }
    
    private void OpenPanel()
    {
      SetStats();

      gameObject.SetActive(true);
      
      SetItems();
      SetDiamondItems();
      SetPurchasableItemsWithDiamond();
    }

    private void SetStats()
    {
      _coinText.text = DataManager.Instance.LoadInt(PlayerPrefKey.Coin).ToString();
      _energyText.text = DataManager.Instance.LoadInt(PlayerPrefKey.Energy).ToString();
      _diamondText.text = DataManager.Instance.LoadInt(PlayerPrefKey.Diamond).ToString();
    }

    private void StatAnimation(TextMeshProUGUI text, int price, PlayerPrefKey playerPrefKey, float duration = 1f)
    {
      int value = DataManager.Instance.LoadInt(playerPrefKey);
      int endValue = value - price;
      
      DOTween.To(() => value, x => 
      {
        value = x;
        text.text = value.ToString();
      }, endValue, duration);
      
      DataManager.Instance.SaveInt(playerPrefKey, endValue);
    }

    private async void ScrollRectAnimation(ScrollRect scrollRect, float animationTime = 0, int delay = 5)
    {
      await Task.Delay(delay);
      
      scrollRect.horizontalNormalizedPosition = 1;
      scrollRect.DOHorizontalNormalizedPos(0f, animationTime).SetEase(Ease.InCubic);
    }

    #region Cars
    
    [Space(15)]
    [Header("Cars")]
    [SerializeField]
    private GameObject _item;

    [SerializeField]
    private Transform _gameProductsPool;

    [SerializeField]
    private ScrollRect _threeDimensionObjectScrollRect;

    private void SetItems()
    {
      Dictionary<int, CarVo> carVos = CarManager.Instance.GetCarData();
      
      for (int i = 0; i < carVos.Count; i++)
      {
        GameObject item = Instantiate(_item, _gameProductsPool);
      
        ShopPanelItem shopPanelItem = item.GetComponent<ShopPanelItem>();

        CarVo carVo = carVos.ElementAt(i).Value;
        shopPanelItem.SetData(carVo.CarObjectUI, carVo.CoinPrice, carVo.DiamondPrice, carVo.Owned, carVo.Id);
      }

      ScrollRectAnimation(_threeDimensionObjectScrollRect, 2.5f);

      //TODO: Safak: Level, Particle, Sound
    }

    public void BuyCar(int id, int diamondReward, int coinPrice)
    {
      StatAnimation(_coinText, coinPrice, PlayerPrefKey.Coin);
      StatAnimation(_diamondText, -diamondReward, PlayerPrefKey.Diamond);
      
      CarManager.Instance.UnlockedCar(id);
    }
    
    #endregion

    #region Buy With Diamond Products
    
    [Space(15)]
    [Header("Purchase With Diamond")]
    [SerializeField]
    private GameObject _inGameProductItem;

    [SerializeField]
    private Transform _inGameProductsPool;

    [SerializeField]
    private ScrollRect _inGameProductsScrollRect;
    
    private const string _inGameProductsDataPath = "Data/InGameProducts/In-Game Products Data";

    private void SetPurchasableItemsWithDiamond()
    {
      InGameProductsData data = Resources.Load<InGameProductsData>(_inGameProductsDataPath);

      List<InGameProductVo> inGameProductsVos = data.InGameProductsVos;

      for (int i = 0; i < inGameProductsVos.Count; i++)
      {
        GameObject item = Instantiate(_inGameProductItem, _inGameProductsPool);
        InGameProductVo inGameProductsVo = inGameProductsVos[i];

        InGameProductItem inGameProductItem = item.GetComponent<InGameProductItem>();
        inGameProductItem.SetData(inGameProductsVo);
      }

      ScrollRectAnimation(_inGameProductsScrollRect);
    }

    public void BuyInGameProduct(InGameProductVo inGameProductVo)
    {
      TextMeshProUGUI targetText;

      switch (inGameProductVo.PlayerPrefKey)
      {
        case PlayerPrefKey.Coin:
          targetText = _coinText;
          break;
        case PlayerPrefKey.Energy:
          targetText = _energyText;
          break;
        default:
          return;
      }
      
      StatAnimation(targetText, -inGameProductVo.Amount, inGameProductVo.PlayerPrefKey);
      StatAnimation(_diamondText, inGameProductVo.Price, PlayerPrefKey.Diamond);
    }

    #endregion

    #region Real Money Products

    [Space(15)]
    [Header("Diamonds")]
    [SerializeField]
    private GameObject _diamondItem;

    [SerializeField]
    private Transform _diamondProductsPool;

    [SerializeField]
    private ScrollRect _diamondProductsScrollRect;
    
    private const string _diamondDataPath = "Data/Diamond/Diamond Data";

    private void SetDiamondItems()
    {
      DiamondData data = Resources.Load<DiamondData>(_diamondDataPath);

      for (int i = 0; i < data.DiamondVos.Count; i++)
      {
        GameObject item = Instantiate(_diamondItem, _diamondProductsPool);
        
        ShopPanelDiamondItem shopPanelDiamondItem = item.GetComponent<ShopPanelDiamondItem>();

        DiamondVo diamondVo = data.DiamondVos[i];
        shopPanelDiamondItem.SetData(diamondVo.DollarPrice, diamondVo.Amount, diamondVo.Sprite);
      }

      ScrollRectAnimation(_diamondProductsScrollRect);
    }

    public void BuyDiamond(float price, int amount)
    {
      // TODO: Open purchase screen.
    }

    #endregion
  }
}