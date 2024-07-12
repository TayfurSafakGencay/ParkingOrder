using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Dreamteck.Splines;
using Enum;
using Managers;
using UnityEngine;

namespace View
{
  public class Car : MonoBehaviour
  {
    private SplineFollower _splineFollower;

    [SerializeField]
    private MeshRenderer _meshRenderer;

    [SerializeField]
    private int _materialIndex;

    private const string _objectTag = "Car";

    private void Awake()
    {
      _splineFollower = gameObject.GetComponent<SplineFollower>();

      gameObject.tag = _objectTag;
      gameObject.GetComponent<BoxCollider>().enabled = false;

      GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateKey gameState)
    {
      if (gameState == GameStateKey.EndGameSuccess)
      {
        OnGameFinishedSuccessfully();
      }
    }

    private void OnCollisionEnter(Collision other)
    {
      if (other.gameObject.CompareTag("Car"))
      {
        CameraManager.Instance.ShakeCamera();

        GameManager.Instance.GameFinished(false);
      }
    }

    private bool _isCarMoving;

    private const float _followSpeed = 3f;

    public void Move(float speed = _followSpeed)
    {
      if (_isCarMoving) return;

      LevelManager.Instance.Move();

      CarManager.Instance.CarMoved(_moveDetectId, gameObject);

      _splineFollower.followSpeed = speed * (int)_splineFollower.direction;

      _isCarMoving = true;
    }

    private void Stop()
    {
      _splineFollower.followSpeed = 0;

      _isCarMoving = false;
    }

    private bool _isReachedEnd;

    private void OnEndReached(double percent)
    {
      if (_isReachedEnd) return;

      ReachPoint(true, Dreamteck.Splines.Spline.Direction.Backward);
    }

    private void OnBeginningReached(double percent)
    {
      if (!_isReachedEnd) return;

      ReachPoint(false, Dreamteck.Splines.Spline.Direction.Forward);
    }

    private void ReachPoint(bool isReachedEnd, Dreamteck.Splines.Spline.Direction direction)
    {
      _isReachedEnd = isReachedEnd;

      ReachAnimation(isReachedEnd);
      Stop();

      CarManager.Instance.CarReached(_moveDetectId);
      LevelManager.Instance.ReachedPoint(isReachedEnd);

      _splineFollower.direction = direction;
    }

    private int _moveDetectId;

    public void InitialCarSettings(SplineComputer splineComputer, Material material)
    {
      SetSpline(splineComputer);
      SetMaterial(material);
      SetInitialSplineSettings();

      transform.name = "Car: " + transform.GetSiblingIndex();
      _moveDetectId = transform.GetSiblingIndex();

      InitialAnimation().OnComplete(() => { gameObject.GetComponent<BoxCollider>().enabled = true; });
    }

    private void OnGameFinishedSuccessfully()
    {
      transform.DOShakeScale(SpecialTimeKey.CarDestroyForSuccessful, 0.5f, 12, 10).OnComplete(() => { Destroy(gameObject); });
    }

    #region Setters

    private void SetInitialSplineSettings()
    {
      _splineFollower.follow = true;
      _splineFollower.followSpeed = 0;
      _splineFollower.SetDistance(0);
      _splineFollower.direction = Dreamteck.Splines.Spline.Direction.Forward;
      _splineFollower.applyDirectionRotation = false;

      _splineFollower.onEndReached += OnEndReached;
      _splineFollower.onBeginningReached += OnBeginningReached;
    }

    private void SetSpline(SplineComputer splineComputer)
    {
      _splineFollower.spline = splineComputer;
    }

    private void SetMaterial(Material material)
    {
      Material[] materials = _meshRenderer.materials;
      materials[_materialIndex] = material;

      _meshRenderer.materials = materials;
    }

    #endregion

    #region Animations

    private TweenerCore<Vector3, Vector3, VectorOptions> InitialAnimation()
    {
      transform.localScale = new Vector3(0, 0, 0);

      return transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.75f).SetEase(Ease.OutBack);
    }

    private void ReachAnimation(bool end)
    {
      int direction = end ? 1 : -1;

      _meshRenderer.transform.DOLocalRotate(new Vector3(5 * direction, 0, 0), 0.3f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    #endregion

    private void OnDestroy()
    {
      GameManager.OnGameStateChanged -= OnGameStateChanged;
    }
  }
}