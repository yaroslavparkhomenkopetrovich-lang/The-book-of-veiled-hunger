using UnityEngine;

namespace Assets.Project.Scripts.Visuals
{
    /// <summary>
    /// Decouples visual transform position from the physics root to achieve
    /// a stroboscopic, low-tick-rate visual effect reminiscnet of earrly 2000s video games.
    /// </summary>
    public class QuantizedPosition : MonoBehaviour
    {
        [Header("Retro Timing")]
        [Tooltip("Visual position update frequence in Hz (e.g., 10 for 10 Hz)")]
        [SerializeField] private float _retroFPS = 16f;

        private Transform _parentRoot;

        private Vector3 _accumulatedWorldOffset;

        private float _timeAccumulator;
        private float _stepInterval;

        private Vector3 _lastSnappedWorldPos;

        private void Awake()
        {
            _parentRoot = transform.parent;

            if (_parentRoot == null)
            {
                Debug.LogError($"{name}: QuantizedPosition requires a parent transform to follow.", this);
                enabled = false;
                return;
            }

            // Calculate the time interval between visual updates based on the specified retro FPS
            _stepInterval = 1f / _retroFPS;

            _lastSnappedWorldPos = _parentRoot.position;
        }

        private void Start()
        {
            transform.SetParent(null); // Detach from parent to avoid inheriting its transform
        }

        private void LateUpdate()
        {
            if (_parentRoot == null)
            {
                Debug.LogError($"{name}: Parent transform is missing. Disabling QuantizedPosition.", this);
                enabled = false;
                Destroy(gameObject); // Optionally destroy the visual object if the parent is missing
                return;
            }
            // Accumulate time since the last visual update
            _timeAccumulator += Time.deltaTime;

            // If enough time has passed, update the visual position
            if (_timeAccumulator >= _stepInterval)
            {
                // Snap the visual position to the parent's position
                _lastSnappedWorldPos = _parentRoot.position;
                // Reset the time accumulator, preserving any leftover time
                _timeAccumulator = 0f;
            }

            transform.position = _lastSnappedWorldPos;
        }
    }
}

