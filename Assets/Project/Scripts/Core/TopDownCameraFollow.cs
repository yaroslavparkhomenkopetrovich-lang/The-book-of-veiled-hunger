using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    [Header("Tracking Target")]
    [SerializeField] private Transform _target;

    [Header("Position & Offset")]
    [SerializeField] private Vector3 _offset = new (0f, 15f, -8f);
    [SerializeField] private float _smoothSpeed5f;

    private void LateUpdate()
    {
        if (_target == null) return;

        Vector3 desiredPosition = _target.position + _offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            _smoothSpeed5f * Time.deltaTime
            );
    }
}
