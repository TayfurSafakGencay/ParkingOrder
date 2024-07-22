using System.Collections.Generic;
using DG.Tweening;
using Enum;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

namespace UI
{
  public class ChestOpeningPanel : MonoBehaviour
  {
    [SerializeField]
    private Image _background;

    [SerializeField]
    private Transform _chest;

    [SerializeField]
    private Button _upgradeButton;

    [SerializeField]
    private List<Material> _materials;

    [SerializeField]
    private Transform _topPiece;

    [SerializeField]
    private Transform _bottomPiece;

    [SerializeField]
    private TextMeshProUGUI _powerUpText;

    [SerializeField]
    private TextMeshProUGUI _titleText;

    private void Awake()
    {
      GameManager.OnGameStateChanged += OnGameStateChanged;

      _meshRenderers.Add(_topPiece.GetComponent<MeshRenderer>());
      _meshRenderers.Add(_bottomPiece.GetComponent<MeshRenderer>());
    }

    private void OnGameStateChanged(GameStateKey gameState)
    {
      if (gameState != GameStateKey.ChestOpening) return;

      OpenPanel();
    }

    private const float _chestScale = 150;

    private const float _chestRotationXScale = 180;

    private const float _chestRotationYScale = 130;

    private const float rotationTime = 0.4f;

    private int _chestLevel;

    public void OnClickPanel()
    {
      _chestLevel++;

      if (_chestLevel == _materials.Count)
      {
        _upgradeButton.interactable = false;

        _powerUpText.transform.DOKill();
        _powerUpText.gameObject.SetActive(false);
        
        PlayChestSound();
        ChestOpeningAnimation();
        return;
      }
      
      PlayChestSound();
      ChestUpgradeAnimation();
      ChangeChestView();
    }

    private bool SetRewards()
    {
      List<int> totalCars = CarManager.Instance.GetCarsCount();
      List<int> unlockedCars = CarManager.Instance.GetUnlockedCars();

      for (int i = 0; i < totalCars.Count; i++)
      {
        int id = totalCars[i];

        if (unlockedCars.Contains(id)) continue;
        if (_lockedCars.Contains(id)) continue;
        _lockedCars.Add(id);
      }

      if (_lockedCars.Count > 1) return true;

      ClosePanelWithoutReward();
      return false;
    }

    private async void OpenPanel()
    {
      if (!SetRewards()) return;

      _background.color = new Color(1, 1, 1, 0);
      _chest.localScale = new Vector3(0, 0, 0);

      _powerUpText.transform.localScale = new Vector3(0, 0, 0);
      _titleText.transform.localScale = new Vector3(0, 0, 0);

      _topPiece.localRotation = Quaternion.Euler(0, 0, 0);

      _chestLevel = 0;
      ChangeChestView();

      _upgradeButton.interactable = false;

      OpenAllPieces();

      Sequence firstSequence = DOTween.Sequence();
      firstSequence.Append(_background.DOFade(1f, 0.75f).SetEase(Ease.Linear));
      firstSequence.Append(_chest.transform.DOScale(new Vector3(150, 150, 150), 0.5f).SetEase(Ease.InQuart)).OnComplete(() => { _upgradeButton.interactable = true; });
      firstSequence.Join(_titleText.transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.InQuart));
      await firstSequence.Join(_powerUpText.transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.InQuart)).AsyncWaitForCompletion();

      _powerUpText.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1f)
        .SetEase(Ease.InOutQuad)
        .SetLoops(-1, LoopType.Yoyo);
    }

    private void ChestUpgradeAnimation()
    {
      _chest.transform.localRotation = Quaternion.Euler(0, 20, 0);

      Sequence upgradeSequence = DOTween.Sequence();
      upgradeSequence.Append(_chest.DORotate(new Vector3(0, -340, 0), rotationTime, RotateMode.FastBeyond360).SetEase(Ease.Linear));
      upgradeSequence.Join(_chest.DOScale(new Vector3(_chestRotationXScale, _chestRotationYScale, _chestScale), rotationTime).SetEase(Ease.Linear));
      upgradeSequence.Append(_chest.DOScale(new Vector3(_chestScale, _chestScale, _chestScale), rotationTime).SetEase(Ease.Linear));
    }

    private void ChestOpeningAnimation()
    {
      Sequence openingSequence = DOTween.Sequence();

      openingSequence.Append(_chest.DOShakePosition(2.5f, 35, 100, 20));
      openingSequence.Append(_chest.DOScale(_chestScale, 0.25f));
      openingSequence.Append(_topPiece.DORotate(new Vector3(120, 20, 0), 1f));
      openingSequence.Join(_chest.DOScaleY(170, 1f)).OnComplete(GetReward);
    }

    private Sequence ChestClosingAnimation()
    {
      Sequence closingSequence = DOTween.Sequence();

      closingSequence.SetDelay(2f);

      closingSequence.Append(_chest.DOShakeScale(2f, 15, 25, 20));
      closingSequence.Append(_topPiece.DORotate(new Vector3(0, 20, 0), 1f));
      closingSequence.Join(_chest.DOScale(new Vector3(150, 150, 150), 1f));
      closingSequence.Append(_chest.DOScale(new Vector3(200, 200, 200), 1f)).SetEase(Ease.InCubic).OnComplete(() => { _chest.gameObject.SetActive(false); });

      return closingSequence;
    }

    private readonly List<MeshRenderer> _meshRenderers = new();

    private void ChangeChestView()
    {
      foreach (MeshRenderer meshRenderer in _meshRenderers)
      {
        meshRenderer.material = _materials[_chestLevel];
      }
    }

    private void OpenAllPieces()
    {
      _chest.gameObject.SetActive(true);
      _topPiece.gameObject.SetActive(true);
      _bottomPiece.gameObject.SetActive(true);
      _powerUpText.gameObject.SetActive(true);
      gameObject.SetActive(true);
    }

    private void PlayChestSound()
    {
      SoundKey soundKey;
      switch (_chestLevel)
      {
        case 1:
          soundKey = SoundKey.Chest_1;
          break;
        case 2:
          soundKey = SoundKey.Chest_2;
          break;
        case 3:
          soundKey = SoundKey.Chest_3;
          break;
        case 4:
          soundKey = SoundKey.Chest_4;
          break;
        case 5:
          soundKey = SoundKey.Chest_5;
          break;
        case 6:
          soundKey = SoundKey.Chest_6;
          break;
        case 7:
          soundKey = SoundKey.Chest_Opening;
          break;
        default:
          return;
      }
      
      SoundManager.Instance.PlaySound(soundKey);
    }
    
    [Header("3D Animation Positions")]
    [SerializeField]
    private Transform _initialPositionFor3DAnimationObject;

    [SerializeField]
    private Transform _firstTargetForAnimation;

    [SerializeField]
    private Transform _secondTargetForAnimation;

    private readonly List<int> _lockedCars = new();

    private async void GetReward()
    {
      int firstCarId = GetRandomCar(_lockedCars);
      _lockedCars.Remove(firstCarId);
      int secondCarId = GetRandomCar(_lockedCars);


      CarManager.Instance.UnlockedCar(firstCarId);
      CarManager.Instance.UnlockedCar(secondCarId);

      Sequence firstSequence = CarAnimation(firstCarId, _firstTargetForAnimation, out _firstCar);
      await firstSequence.Append(CarAnimation(secondCarId, _secondTargetForAnimation, out _secondCar)).AsyncWaitForCompletion();

      CarRotationAnimation(_firstCar);
      CarRotationAnimation(_secondCar);

      await ChestClosingAnimation().AsyncWaitForCompletion();

      PanelClosingAnimations();
    }

    private int GetRandomCar(List<int> lockedCars)
    {
      System.Random random = new();
      int randomIndex = random.Next(lockedCars.Count);
      int carId = lockedCars[randomIndex];
      return carId;
    }

    private void PanelClosingAnimations()
    {
      _firstCar.transform.DOScale(new Vector3(0, 0, 0), 1f).SetEase(Ease.InCubic);
      _secondCar.transform.DOScale(new Vector3(0, 0, 0), 1f).SetEase(Ease.InCubic).OnComplete(() =>
      {
        _background.DOFade(0, 0.5f);

        _firstCar.transform.DOKill();
        Destroy(_firstCar);

        _secondCar.transform.DOKill();
        Destroy(_secondCar);

        gameObject.SetActive(false);
      });
    }

    private void ClosePanelWithoutReward()
    {
      _background.DOFade(0, 0.5f);

      gameObject.SetActive(false);
    }

    private const float _carAnimationTime = 1f;

    private GameObject _firstCar;

    private GameObject _secondCar;

    private Sequence CarAnimation(int id, Transform target, out GameObject carGameObject)
    {
      Vector3 initialScale = new(75, 75, 75);
      Vector3 lastScale = new(150, 150, 150);
      Quaternion initialRotation = Quaternion.Euler(0, -70, 0);

      GameObject carObjectUI = CarManager.Instance.GetCarData()[id].CarObjectUI;

      GameObject car = Instantiate(carObjectUI, _initialPositionFor3DAnimationObject.position, initialRotation, _background.transform);
      car.transform.localScale = initialScale;
      car.transform.localRotation = initialRotation;
      carGameObject = car;

      Sequence carSequence = DOTween.Sequence();
      carSequence.Append(car.transform.DOMove(target.position, _carAnimationTime).SetEase(Ease.InCubic));
      carSequence.Join(car.transform.DOScale(lastScale, _carAnimationTime).SetEase(Ease.InCubic));

      return carSequence;
    }

    private const float _rotationAnimationTime = 1f;

    private void CarRotationAnimation(GameObject car)
    {
      car.transform.DORotate(new Vector3(0, 360, 0), _rotationAnimationTime, RotateMode.FastBeyond360)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Incremental);
    }
  }
}