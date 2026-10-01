using UnityEngine;

namespace Assets.Project.Scripts.Combat
{
    public class MovementAffector : MonoBehaviour
    {
        private float _stunUntilTime;
        private float _speedMultiplier = 1f;
        private Vector3 _knockbackVelocity;

        // --- Read by movement ---
        public bool IsStunned => Time.time < _stunUntilTime;
        public float SpeedMultiplier => Mathf.Max(0f, _speedMultiplier);
        public bool HasKnockback => _knockbackVelocity.sqrMagnitude >= 0.001f;
        public Vector3 KnockbackVelocity => _knockbackVelocity;

        // --- Written by combat/status ---
        public void ApplyStun(float duration)
        {
            if (duration <= 0f) return;

            _stunUntilTime = Mathf.Max(_stunUntilTime, Time.time + duration);
        }

        /// <summary>
        /// Queues planar knockback for movement to consume once.
        /// </summary>
        public void ApplyKnockback(Vector3 direction, float force)
        {
            direction.y = 0f;

            if (force <= 0f || direction.sqrMagnitude < 0.001f) return;

            _knockbackVelocity += direction.normalized * force;
        }
        public void ApplyHitImpulse(ImpulseResult impulse) // Stun plus knockback
        {
            ApplyStun(impulse.StunDuration);
            ApplyKnockback(impulse.Direction, impulse.Force);
        }

        public void SetSpeedMultiplier(float multiplier)
        {
            _speedMultiplier = Mathf.Max(0f, multiplier);
        }

        public void ResetSpeedMultiplier()
        {
            _speedMultiplier = 1f;
        }

        // --- Consumed once by movement ---
        /// <summary>
        /// Movement reads and clears knockback when it applies it.
        /// </summary>
        public void TickKnockback(float deltaTime, float drag = 10f)
        {
            _knockbackVelocity = Vector3.MoveTowards
                (
                    _knockbackVelocity,
                    Vector3.zero, drag * deltaTime
                );
        }
    }
}