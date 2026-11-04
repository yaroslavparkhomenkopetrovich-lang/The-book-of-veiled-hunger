using Assets.Project.Scripts.Combat;
using Assets.Project.Scripts.Status;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Project.Scripts.Environment
{
    [RequireComponent(typeof(Collider))]
    public class HazardZone : MonoBehaviour
    {
        [SerializeField] private HazardZoneData _data;
        public HazardZoneData Data => _data;

        // Pre-collider no GC per frame
        private readonly Dictionary<Collider, float> _nextPulseTime = new();

        private void Reset()
        {
            var coll = GetComponent<Collider>();
            coll.isTrigger = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (_data == null || _data.HitEffects == null) return;

            float now = Time.time;

            if (_nextPulseTime.TryGetValue(other, out float next) && now < next)
                return;

            _nextPulseTime[other] = now + _data.PulseInterval;

            // Status-only path
            if (!_data.DealHitDamage && _data.ApplyStatusEffects)
            {
                if (!other.TryGetComponent<StatusEffectController>(out var statuses))
                    return;

                var requests = StatusEffectRequestFactory.FromPayloads
                    (
                        _data.HitEffects.StatusEffects,
                        gameObject
                    );

                statuses.ApplyAllStatusEffects(requests);
                return;
            }

            // Damage (+optional status) - path same as gun/melee
            if (!other.TryGetComponent<IDamageable>(out var damageable))
                return;

            DamageInfo info = DamageInfo.FromHazardZone
                (
                    _data,
                    gameObject,
                    other.ClosestPoint(transform.position)
                );

            damageable.TakeDamage(info);
        }

        private void OnTriggerExit(Collider other)
        {
            _nextPulseTime.Remove(other);
        }
    }
}

