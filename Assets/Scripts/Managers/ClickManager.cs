using Enum;
using UnityEngine;
using View;

namespace Managers
{
  public class ClickManager : MonoBehaviour
  {
    [SerializeField]
    private Camera _camera;
    
    private const string _car = "Car";
    private void Update()
    {
      if (GameManager.GameStateKey != GameStateKey.InGame) return;

      if (!Input.GetMouseButtonDown(0)) return;
      Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

      if (!Physics.Raycast(ray, out RaycastHit hit)) return;
      if (!hit.collider.CompareTag(_car)) return;

      bool hasItCarComponent = hit.collider.TryGetComponent(out Car car);

      if (!hasItCarComponent)
      {
        Debug.LogError("Object has not Car component!");        
        return;
      }
      
      car.Move();
    }
  }
}