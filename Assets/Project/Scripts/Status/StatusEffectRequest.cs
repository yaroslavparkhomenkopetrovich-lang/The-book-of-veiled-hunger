using UnityEngine;

namespace Assets.Project.Scripts.Status
{
    public readonly struct StatusEffectRequest
    {
        public readonly StatusEffectType Type;
        public readonly float Duration;         // Seconds
        public readonly float Power;            // Meaning depends on type
        public readonly int Stacks;             // Usually 1 on apply
        public readonly GameObject Source;      // Who/what applied

        public StatusEffectRequest
            (
                StatusEffectType type,
                float duration,
                float power,
                GameObject source = null,
                int stacks = 1
            )
        {
            Type = type;
            Duration = duration;
            Power = power;
            Source = source;
            Stacks = stacks;
        }
    }
}
