using UnityEngine;

namespace Assets.Project.Scripts.Combat
{
    public class ImpulseProcessor : MonoBehaviour
    {
        /// <summary>
        /// Returns resolved planar vector and stun
        /// </summary>
        public ImpulseResult CalculateImpulse
            (
                DamageInfo info,
                Vector3 targetPosition,
                ImpulseType impulseType,
                float force,
                float stunDuration
            )
        {
            if (force <= 0f && stunDuration <= 0f)
            {
                return new ImpulseResult(Vector3.zero, 0f, 0f);
            }

            Vector3 rawDirection = Vector3.zero;

            switch (impulseType)
            {
                case ImpulseType.PushAwayFromBullet:
                    rawDirection = info.HitDirection;
                    break;

                case ImpulseType.PushAwayFromImpact:
                    rawDirection = targetPosition - info.HitPoint;
                    break;

                case ImpulseType.PullTowardImpact:
                    rawDirection = info.HitPoint - targetPosition;
                    break;

                case ImpulseType.PullTowardShooter:
                    if (info.Instigator != null)
                    {
                        rawDirection = info.Instigator.transform.position - targetPosition;
                    }
                    break;
            }

            // Keep physical impulse strictly plannar
            rawDirection.y = 0f;

            Vector3 finalDirection = rawDirection.sqrMagnitude > 0.001f ? rawDirection.normalized : Vector3.zero;

            return new ImpulseResult(finalDirection, force,stunDuration);
        }
    }
}
