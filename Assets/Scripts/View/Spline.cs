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
      
      SubscribeActions();
    }

    private void SubscribeActions()
    {
      GameManager.OnGameStarted += OnGameStarted;
    }

    private void OnGameStarted()
    {
      SplineManager.Instance.AddSpline(_splineComputer, this);

    }

    public void SetColors(Color color)
    {
      // ColorAllTheDots(color);
      ColorParkAreaLines(color);
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