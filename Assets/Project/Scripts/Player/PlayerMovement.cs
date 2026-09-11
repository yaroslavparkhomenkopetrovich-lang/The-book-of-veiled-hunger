using UnityEngine;

namespace Assets.Project.Scripts.Player
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 6f;
        private CharacterController _charController;

        private void Awake()
        {
            _charController = GetComponent<CharacterController>();
        }

        public void MoveTowards(Vector3 direction)
        {
            direction.y = 0f; // Ensure movement is only in the XZ plane

            Vector3 motion = Vector3.zero;

            if (direction.sqrMagnitude > 0.01f) // Check if the movement vector is significant
            {
               motion = direction.normalized * (_speed * Time.deltaTime);
            }

            // Always apply persistent downward acceleration
            motion.y = -9.8f * Time.deltaTime;

             _charController.Move(motion);
        }

        public void RotateTowards(Vector3 targetDir)
        {
           
            targetDir.y = transform.position.y; // Keep the y position the same to avoid tilting the player
            Vector3 lookDir = targetDir - transform.position;

            // Rotate the player to face the direction of movement
            if (lookDir.sqrMagnitude > 0.01f) // Check if the look direction vector is significant
            {
                transform.rotation = Quaternion.LookRotation(lookDir);
            }
        }
    }
}
