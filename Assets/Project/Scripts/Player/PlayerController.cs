using Unity.VisualScripting;
using UnityEngine;
using Assets.Project.Scripts.Weapons;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 8f;

    [Header("Aim Settings")]
    [SerializeField] private LayerMask _aimLayerMask;

    [Header("Weapon Attachment")]
    [SerializeField] private WeaponController _weaponController;

    private Rigidbody _rigidbody;
    private Camera _mainCamera;
    private Vector3 _movementInput;
    private Vector3 _lookTarget;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;

        // Enable physics interpolation for smoother movement
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        // Configure physics properties for responsive arcade control
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {
        GatherInput();
        CalculateAimDirection();
        HandleFiringInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void GatherInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Map 2D inputs to the 3D XZ ground plane and normalize
        _movementInput = new Vector3(horizontal, 0f, vertical).normalized;
    }

    private void CalculateAimDirection()
    {
        if (_mainCamera == null) return;

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new (Vector3.up, new Vector3(0f, transform.position.y, 0f));

        if (groundPlane.Raycast(ray, out float enterDistance))
        {
            _lookTarget = ray.GetPoint(enterDistance);
            _lookTarget.y = transform.position.y; // Keep rotation level on the horizontal plan
        }
    }

    private void HandleFiringInput()
    {
        // Continuous fire when holding left click
        if (Input.GetMouseButton(0) && _weaponController != null)
        {
            _weaponController.TryShoot();
        }
    }

    private void MovePlayer()
    {
        Vector3 targetVelocity = _movementInput * _moveSpeed;
        _rigidbody.linearVelocity = new Vector3(targetVelocity.x, 0f, targetVelocity.z);
    }

    private void RotatePlayer()
    {
        Vector3 directionToTarget = _lookTarget - transform.position;
        directionToTarget.y = 0f; // Keep rotation level on the horizontal plane

        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            _rigidbody.MoveRotation(targetRotation);
        }
    }
}
