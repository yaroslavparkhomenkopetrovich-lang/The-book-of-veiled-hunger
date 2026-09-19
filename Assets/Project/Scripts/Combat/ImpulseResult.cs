using UnityEngine;

namespace Assets.Project.Scripts.Combat
{
    public readonly struct ImpulseResult
    {
        public readonly Vector3 Direction;
        public readonly float Force;
        public readonly float StunDuration;

        public ImpulseResult(Vector3 direction, float force, float stunDuration)
        {
            Direction = direction;
            Force = force;
            StunDuration = stunDuration;
        }
    }
}
