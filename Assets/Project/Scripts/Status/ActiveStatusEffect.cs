using Assets.Project.Scripts.Combat;
using UnityEngine;

namespace Assets.Project.Scripts.Status
{
    public class ActiveStatusEffect
    {
        [SerializeField] private StatusEffectType _type;
        public StatusEffectType Type => _type;
        [SerializeField] private float _remainingDuration;
        public float RemainingDuration => _remainingDuration;
        [SerializeField] private float _power;
        public float Power => _power;
        [SerializeField] private float _tickInterval;
        public float TickInterval => _tickInterval;
        [SerializeField] private float _tickTimer;
        public float TickTimer => _tickTimer;
        [SerializeField] private GameObject _source;
        public GameObject Source => _source;

        public ActiveStatusEffect(StatusEffectRequest request)
        {
            _type = request.Type;
            _remainingDuration = request.Duration;
            _power = request.Power;
            _tickInterval = Mathf.Max(0.05f, request.TickInterval);
            _tickTimer = 0f;
            _source = request.Source;
        }

        /// <summary>
        /// Advances timers. Returns false if the effect has expired and should be removed.
        /// </summary>
        public bool Tick(float deltaTime, DamageProcessor damageProcessor)
        {
            if (_remainingDuration <= 0f) return false; // Effect expired

            _remainingDuration -= deltaTime;

            if (_remainingDuration <= 0f) return false;

            _tickTimer += deltaTime;

            if (_tickTimer >= _tickInterval)
            {
                _tickTimer = 0f; // Reset tick timer
                ApplyTick(damageProcessor);
            }

            return _remainingDuration > 0f;
        }

        private void ApplyTick(DamageProcessor damageProcessor)
        {
            // Stage 3: only bleed for now, but can be expanded to other effects in the future
            if (_type != StatusEffectType.Bleed) return;
            if (damageProcessor == null) return;

            int damageAmount = Mathf.Max(1, Mathf.RoundToInt(_power));
            damageProcessor.ApplyDamage(damageAmount, _source);
        }
    }
}
