using Cinemachine;
using UnityEngine;

namespace Managers
{
  public class CameraManager : MonoBehaviour
  {
    public static CameraManager Instance;
    
    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private const float _shakeIntensity = 2f;

    private const float _shakeTime = 0.3f;

    private float _timer;

    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      
      _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
      _cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
      StopShake();
    }

    public void ShakeCamera()
    {
      _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _shakeIntensity;

      _timer = _shakeTime;
    }

    private void StopShake()
    {
      _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;

      _timer = 0;
    }

    private void Update()
    {
      if (!(_timer > 0)) return;
      _timer -= Time.deltaTime;

      if (_timer <= 0) StopShake();
    }
  }
}