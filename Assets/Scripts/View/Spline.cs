using System;
using System.Collections.Generic;
using Dreamteck.Splines;
using Enum;
using Managers;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

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

      GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateKey gameState)
    {
      if (gameState == GameStateKey.InGame)
      {
        OnGameStarted();
      }
    }

    private async void OnGameStarted()
    {
      await Task.Delay(SpecialTimeKey.WaitLevelData);
      
      SplineManager.Instance.AddSpline(_splineComputer, this);
    }

    public void SetColors(Color color)
    {
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

    private void OnDestroy()
    {
      GameManager.OnGameStateChanged -= OnGameStateChanged;
    }
  }
}