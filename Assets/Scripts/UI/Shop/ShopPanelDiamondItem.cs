using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
  public class ShopPanelDiamondItem : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI _priceText;

    [SerializeField]
    private TextMeshProUGUI _titleText;

    [SerializeField]
    private Image _productImage;

    [SerializeField]
    private Button _buyButton;

    private Sprite _sprite { get; set; }

    private float _price { get; set; }

    private int _amount { get; set; }

    public void SetData(float price, int amount, Sprite sprite)
    {
      _price = price;
      _amount = amount;
      _sprite = sprite;

      SetUI();
    }

    public void BuyProduct()
    {
      PanelManager.Instance.ShopPanel.BuyDiamond(_price, _amount);
    }

    private void SetUI()
    {
      _priceText.text = _price + "$";
      _titleText.text = "x" + _amount;

      _productImage.sprite = _sprite;
    }
  }
}