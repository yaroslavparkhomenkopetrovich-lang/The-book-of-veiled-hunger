using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Project.Scripts.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [Header("Aiming Settings")]
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _groundLayer;

        // Public properties ready to be read by other scripts
        public Vector3 MoveInput { get; private set; }
        public Vector3 AimWorldPosition { get; private set; }
        public bool IsFirePressed { get; private set; }

        private void Awake()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }
        }

        private void Update()
        {
            ReadMovementInput();
            ReadAimInput();
            ReadFireInput();
        }

        private void ReadMovementInput()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            Vector3 move = Vector3.zero; // Reset movement vector
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) move.z += 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) move.z -= 1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) move.x += 1f;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) move.x -= 1f;

            MoveInput = move.normalized; // Normalize to prevent faster diagonal movement
        }

        private void ReadAimInput()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;

            Ray ray = _camera.ScreenPointToRay(mouse.position.ReadValue()); // Create a ray from the camera to the mouse position
            if (Physics.Raycast(ray, out RaycastHit hit, 300f, _groundLayer)) // Check if the ray hits the ground layer
            {
                AimWorldPosition = hit.point;
            }
        }

        private void ReadFireInput()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;

            // Held down for full auto or check .wasPressedThisFrame for semi-auto
            IsFirePressed = mouse.leftButton.isPressed;
        }
    }
}
