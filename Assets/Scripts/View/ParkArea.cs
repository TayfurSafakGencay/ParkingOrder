using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;

namespace View
{
  public class ParkArea : MonoBehaviour
  {
    private void Start()
    {
      SplineFollower splineFollower =  gameObject.GetComponent<SplineFollower>();
      splineFollower.follow = true;

      InitialAnimation();
    }
    
    private void InitialAnimation()
    {
      transform.localScale = new Vector3(0, 0, 0);
      transform.DOScale(new Vector3(1, 1, 1), 0.75f).SetEase(Ease.OutBack);
    }
  }
}