using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    [Header("Tracking Target")]
    [SerializeField] private Transform _target;

    [Header("Position & Offset")]
    [SerializeField] private Vector3 _offset = new (0f, 15f, -8f);
    [SerializeField] private float _smoothSpeed = 5f;

    private void LateUpdate()
    {
        if (_target == null) return;

        // 1) Calculate the desired position based on the target's position and the offset
        Vector3 desiredPosition = _target.position + _offset;

        // 2) Smoothly interpolate the camera's position towards the desired position
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            _smoothSpeed * Time.deltaTime
            );

        // 3) Keep camera pointed at the target
        // transform.LookAt(_target.position);
    }
}
