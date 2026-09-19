using UnityEngine;

namespace Assets.Project.Scripts.Visuals
{
    [RequireComponent(typeof(Animator))]
    public class SteppedAnimator : MonoBehaviour
    {
        [Tooltip("Target visual animation rate (e.g., 12-16 FPS)")]
        [SerializeField] private float _retroFPS = 14f;

        private Animator _animator;
        private float _timeAccumulator = 0f;
        private float _stepInterval;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            if (_animator == null)
            {
                Debug.LogError("SteppedAnimator requires an Animator component. Script is disabled.");
                enabled = false; // Disable the script if Animator is missing
                return;
            }

            _animator.enabled = false; // Disable automatic animation updates

            _stepInterval = 1f / _retroFPS;
        }

        private void LateUpdate()
        {
            _timeAccumulator += Time.deltaTime; // Accumulate time since the last update

            if (_timeAccumulator >= _stepInterval)
            {
                // Update the animator manually at fixed intervals
                _animator.Update(_timeAccumulator); 
                _timeAccumulator = 0f; // Reset the accumulator after updating
            }
        }
    }
}
