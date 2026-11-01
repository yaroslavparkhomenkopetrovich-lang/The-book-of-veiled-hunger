using UnityEngine;

namespace Assets.Project.Scripts.Status
{
    [System.Serializable]
    public class StatusEffectPayload
    {
        [SerializeField] private StatusEffectType _type = StatusEffectType.Bleed;
        public StatusEffectType Type => _type;
        [SerializeField] private float _duration = 3f;
        public float Duration => _duration;
        [SerializeField] private float _power = 5f;
        public float Power => _power;
        [SerializeField] private float _tickInterval = 0.05f;
        public float TickInterval => _tickInterval;
        [SerializeField] private int _stacks = 1;
        public int Stacks => _stacks;
    }
}
