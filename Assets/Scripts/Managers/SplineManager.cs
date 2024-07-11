using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using Spline = View.Spline;

namespace Managers
{
  public class SplineManager : MonoBehaviour
  {
    public static SplineManager Instance;

    [SerializeField]
    private Material _material;

    [SerializeField]
    private List<Color> _colors;

    private void Awake()
    {
      if (Instance == null) Instance = this;
    }

    public void AddSpline(SplineComputer splineComputer, Spline spline)
    {
      Material material = new(_material);
      Color color = GetRandomColor();
      material.color = color;

      spline.SetColors(color);
      
      CarManager.Instance.CreateCar(splineComputer, material);
    }

    private Color GetRandomColor()
    {
      System.Random random = new();
      int index = random.Next(_colors.Count);
      return _colors[index];
    }

  }
}