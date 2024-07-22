using Data;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
  public class InGameProductItem : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI _priceText;

    [SerializeField]
    private TextMeshProUGUI _amountText;

    [SerializeField]
    private Image _image;
    
    private InGameProductVo _inGameProductVo;
    
    public void SetData(InGameProductVo inGameProductVo)
    {
      _inGameProductVo = inGameProductVo;
      
      UpdateUI();
    }

    private void UpdateUI()
    {
      _priceText.text = _inGameProductVo.Price.ToString();
      _amountText.text = "x" + _inGameProductVo.Amount;
      _image.sprite = _inGameProductVo.Sprite;
    }

    public void BuyItem()
    {
      PanelManager.Instance.ShopPanel.BuyInGameProduct(_inGameProductVo);
    }
  }
}