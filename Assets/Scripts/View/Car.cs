using Dreamteck.Splines;
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
        }

        private bool _isCarMoving;

        private const float _followSpeed = 3f;
        
        public void Move(float speed = _followSpeed)
        {
            if(_isCarMoving) return;
            
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
            if(_isReachedEnd) return;

            _isReachedEnd = true;
            
            Stop();

            _splineFollower.direction = Dreamteck.Splines.Spline.Direction.Backward;
        }

        private void OnBeginningReached(double percent)
        {
            if (!_isReachedEnd) return;
            
            _isReachedEnd = false;

            Stop();

            _splineFollower.direction = Dreamteck.Splines.Spline.Direction.Forward;
        }

        public void InitialSplineSettings(SplineComputer splineComputer, Material material)
        {
            SetSpline(splineComputer);
            SetMaterial(material);
            SetInitialSplineSettings();
        }

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
    }
}
