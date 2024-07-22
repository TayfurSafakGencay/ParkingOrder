using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
  [CreateAssetMenu(fileName = "Diamond Data", menuName = "Create/Create Diamond Data", order = 0)]
  public class DiamondData : ScriptableObject
  {
    public List<DiamondVo> DiamondVos;
  }

  [Serializable]
  public struct DiamondVo
  {
    public int Amount;

    public float DollarPrice;

    public Sprite Sprite;
  }
}