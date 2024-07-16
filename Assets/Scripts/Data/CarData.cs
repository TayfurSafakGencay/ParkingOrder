using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
  [CreateAssetMenu(fileName = "Car Data", menuName = "Create Car Data", order = 0)]
  public class CarData : ScriptableObject
  {
    public List<CarVo> CarFeatures;
  }

  [Serializable]
  public struct CarVo
  {
    public GameObject CarObject;

    [Range(3, 7)]
    public int Speed;

    [Min(0)]
    public int Id;

    public bool Owned;
  }
}