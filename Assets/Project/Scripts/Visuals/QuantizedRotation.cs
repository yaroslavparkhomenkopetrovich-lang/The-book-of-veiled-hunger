using UnityEngine;

namespace Assets.Project.Scripts.Visuals
{
    public class QuantizedRotation : MonoBehaviour
    {
        [Tooltip("The number of fixed directions (8 for 45-degree increments)")]
        [SerializeField] private int _fixedDirections = 8;

        private Transform _parentRoot;

        private void Awake()
        {
            _parentRoot = transform.parent;

            if (_parentRoot == null)
            {
                Debug.LogWarning("QuantizedRotation requires a parent transform to determine rotation.");
            }
        }

        private void LateUpdate()
        {
            if (_parentRoot == null) return;

            Vector3 parentForward = _parentRoot.forward;

            // Convert 3D direction to 2D by ignoring the y component
            float rawAngle = Mathf.Atan2(parentForward.x, parentForward.z) * Mathf.Rad2Deg;
            // Determine the step angle based on the number of fixed directions
            float stepAngle = 360f / _fixedDirections;
            // Snap the raw angle to the nearest step angle
            float snappedAngle = Mathf.Round(rawAngle / stepAngle) * stepAngle;

            // Apply the snapped rotation to the object, keeping it upright (y-axis rotation only)
            transform.rotation = Quaternion.Euler(0f, snappedAngle, 0f);
        }
    }
}
