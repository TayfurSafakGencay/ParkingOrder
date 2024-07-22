using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Data
{
  [CreateAssetMenu(fileName = "In-Game Products Data", menuName = "Create/Create In-Game Products Data", order = 0)]
  public class InGameProductsData : ScriptableObject
  {
    public List<InGameProductVo> InGameProductsVos;
  }

  [Serializable]
  public struct InGameProductVo
  {
    public PlayerPrefKey PlayerPrefKey;
    
    public int Price;

    public int Amount;

    public Sprite Sprite;
  }
}