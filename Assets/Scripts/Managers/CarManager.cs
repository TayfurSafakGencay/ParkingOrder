using System;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using View;
using Random = System.Random;

namespace Managers
{
  public class CarManager : MonoBehaviour
  {
    public static CarManager Instance;

    [SerializeField]
    private List<GameObject> _cars;

    [SerializeField]
    private Transform _carPool;

    private void Awake()
    {
      if (Instance == null)
        Instance = this;
    }

    public void CreateCar(SplineComputer splineComputer, Material material)
    {
      GameObject carInstantiate = Instantiate(GetRandomCar(), Vector3.zero, Quaternion.identity, _carPool);

      Car car = carInstantiate.GetComponent<Car>();
      car.InitialSplineSettings(splineComputer, material);
    }

    private GameObject GetRandomCar()
    {
      Random random = new ();
      int index = random.Next(_cars.Count);
      return _cars[index];
    }
  }
}