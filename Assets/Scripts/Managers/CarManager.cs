using System.Collections.Generic;
using System.Linq;
using Data;
using Dreamteck.Splines;
using Enum;
using UnityEngine;
using View;
using Random = System.Random;

namespace Managers
{
  public class CarManager : MonoBehaviour
  {
    public static CarManager Instance;

    [SerializeField]
    private Transform _carPool;

    private readonly Dictionary<int, GameObject> _movingCars = new();

    private readonly Dictionary<int, CarVo> _cars = new();

    private readonly List<int> _ownedCarIds = new();

    private void Awake()
    {
      if (Instance == null)
        Instance = this;
      
      LoadCarData();

      GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateKey gameState)
    {
      if (gameState != GameStateKey.InGame)
      {
        _movingCars.Clear();
      }
    }

    public void CreateCar(SplineComputer splineComputer, Material material)
    {
      CarVo carVo = GetRandomCar();
      GameObject carInstantiate = Instantiate(carVo.CarObject, Vector3.zero, Quaternion.identity, _carPool);

      Car car = carInstantiate.GetComponent<Car>();
      car.InitialCarSettings(splineComputer, material, carVo.Speed);
    }

    private CarVo GetRandomCar()
    {
      Random random = new();
      int randomIndex = random.Next(_ownedCarIds.Count);
      int index = _ownedCarIds[randomIndex];

      return _cars[index];
    }

    public void CarMoved(int id, GameObject car)
    {
      _movingCars.Add(id, car);
    }

    public void CarReached(int id)
    {
      if (_movingCars.ContainsKey(id))
      {
        _movingCars.Remove(id);
      }
    }

    public int GetMovingCarCount()
    {
      return _movingCars.Count;
    }
    
    private const string _dataPath = "Data/Car/Car Data";
    private void LoadCarData()
    {
      CarData data = Resources.Load<CarData>(_dataPath);
      List<CarVo> carData = data.CarFeatures;
      
      for (int i = 0; i < carData.Count; i++)
      {
        CarVo carVo = carData[i];
        _cars.Add(carVo.Id, carVo);

        if (!carVo.Owned) continue;
        _ownedCarIds.Add(carVo.Id);
      }
    }

    public void UnlockedCar(int id)
    {
      _ownedCarIds.Add(id);
    }

    public List<int> GetUnlockedCars()
    {
      return _ownedCarIds;
    }

    public List<int> GetCarsCount()
    {
      return _cars.Keys.ToList();
    }
  }
}