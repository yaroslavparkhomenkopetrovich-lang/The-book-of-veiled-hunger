using Assets.Project.Scripts.Weapons;
using UnityEngine;

namespace Assets.Project.Scripts.Environment
{
    [CreateAssetMenu(fileName = "Hazard_", menuName = "Combat/Hazard Zone Data")]
    public class HazardZoneData : ScriptableObject
    {
        [SerializeField] private string _zoneName = "Acid Pool";
        public string ZoneName => _zoneName;

        [Tooltip("Seconds between each damage tick")]
        [SerializeField] private float _pulseInterval = 0.5f;
        public float PulseInterval => _pulseInterval;

        [SerializeField] private bool _dealHitDamage = true;
        public bool DealHitDamage => _dealHitDamage;

        [SerializeField] private bool _applyStatusEffects = true;
        public bool ApplyStatusEffects => _applyStatusEffects;

        [Header("Hit Effects")]
        [SerializeField] private HitEffectProfile _hitEffects;
        public HitEffectProfile HitEffects => _hitEffects;
    }
}
