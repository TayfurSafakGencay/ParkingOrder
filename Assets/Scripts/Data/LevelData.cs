using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
  [CreateAssetMenu(fileName = "Level Data", menuName = "Create Level Data", order = 0)]
  public class LevelData : ScriptableObject
  {
    public List<LevelVo> LevelVos;
  }

  [Serializable]
  public struct LevelVo
  {
    public int Level;

    public int MovesCount;

    public int Time;
  }
}