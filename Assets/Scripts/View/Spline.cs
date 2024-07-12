using System.Collections.Generic;
using Dreamteck.Splines;
using Managers;
using UnityEngine;

namespace View
{
  public class Spline : MonoBehaviour
  {
    private SplineComputer _splineComputer;

    [SerializeField]
    private List<MeshRenderer> _parkAreaLines;

    private void Awake()
    {
      _splineComputer = gameObject.GetComponent<SplineComputer>();
      gameObject.name = "Spline: " + transform.GetSiblingIndex();
    }

    private void OnEnable()
    {
      SplineManager.Instance.AddSpline(_splineComputer, this);

    }

    public void SetColors(Color color)
    {
      ColorParkAreaLines(color);
      ColorAllTheDots(color);
    }
    
    private void ColorAllTheDots(Color color)
    {
      SplinePoint[] splinePoints = _splineComputer.GetPoints();

      for (int i = 0; i < splinePoints.Length; i++)
      {
        _splineComputer.SetPointColor(i, color);
      }
    }

    private void ColorParkAreaLines(Color color)
    {
      for (int i = 0; i < _parkAreaLines.Count; i++)
      {
        _parkAreaLines[i].material.color = color;
      }
    }
  }
}