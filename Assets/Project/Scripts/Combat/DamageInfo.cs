using Assets.Project.Scripts.Status;
using Assets.Project.Scripts.Weapons;
using Assets.Project.Scripts.Environment;
using UnityEngine;

namespace Assets.Project.Scripts.Combat
{
    public readonly struct DamageInfo
    {
        // Core
        public readonly int Amount;                     // Raw damage value
        public readonly DamageType Type;                // Elemental, psysical, etc
        public readonly GameObject Instigator;          // Atacker for kills life steal

        // Hit geometry (e.g. VFX, Impulse)
        public readonly Vector3 HitPoint;               // World position for hit effects
        public readonly Vector3 HitNormal;              // Surface normal for decal alignment

        public readonly Vector3 HitDirection;           // Bullet flight direction


        // Hit reaction (impulse)
        public readonly float StunDuration;
        public readonly float KnockbackForce;
        public readonly ImpulseType ImpulseType;

        // Optional status payloads applied AFTER the hit resolves
        public readonly StatusEffectRequest[] StatusEffects;

        // Origin of the threat

        public readonly DamageSource Source;

        public DamageInfo
            (
                int amount,
                DamageType type,
                GameObject instigator,
                Vector3 hitPoint,
                Vector3 hitNormal,
                Vector3 hitDirection = default,
                float stunDuration = 0f,
                float knockbackForce = 0f,
                ImpulseType impulseType = ImpulseType.PushAwayFromBullet,
                DamageSource source = DamageSource.DirectHit,
                StatusEffectRequest[] statusEffects = null
            )
        {
            Amount = amount;
            Type = type;
            Instigator = instigator;
            HitPoint = hitPoint;
            HitNormal = hitNormal;
            HitDirection = hitDirection;
            StunDuration = stunDuration;
            KnockbackForce = knockbackForce;
            ImpulseType = impulseType;
            StatusEffects = statusEffects;
            Source = source;
        }

        public bool HasStatusEffect =>
            StatusEffects != null && StatusEffects.Length > 0;

        // -------------------- Constructor helpers ----------------------------

        public static DamageInfo FromProjectile
            (
                WeaponData weapon,
                GameObject instigator,
                Transform projectileTransform,
                Collider hitCollider
            )
        {
            HitEffectProfile hit = weapon.HitEffects;
            // If the weapon has a hit effect profile, use its values instead of the weapon's base values

            // Founds a point on the collider surface closest to the projectile's position on hit
            Vector3 hitPoint = hitCollider.ClosestPoint(projectileTransform.position);
            Vector3 hitNormal = (projectileTransform.position - hitPoint).normalized;

            return new DamageInfo
            (
                amount: hit.Damage,
                type: hit.DamageKind,
                instigator: instigator,
                hitPoint: hitPoint,
                hitNormal: hitNormal,
                hitDirection: projectileTransform.forward,
                stunDuration: hit.StunDuration,
                knockbackForce: hit.KnockbackForce,
                impulseType: hit.ImpulseKind,
                source: DamageSource.DirectHit,
                statusEffects: StatusEffectRequestFactory.FromPayloads(hit.StatusEffects, instigator)
            );
        }

        public static DamageInfo FromMelee
            (
                int amount,
                DamageType type,
                GameObject attacker,
                Vector3 hitPoint,
                Vector3 hitNormal = default,
                float stunDuration = 0f,
                float knockbackForce = 0f,
                StatusEffectPayload[] onHitEffects = null
            )
        {
            if (hitNormal == default)
                hitNormal = Vector3.up;

            return new DamageInfo
                (
                    amount: amount,
                    type: type,
                    instigator: attacker,
                    hitPoint: hitPoint,
                    hitNormal: hitNormal,
                    hitDirection: default,
                    stunDuration: stunDuration,
                    knockbackForce: knockbackForce,
                    impulseType: ImpulseType.PushAwayFromImpact,
                    source: DamageSource.DirectHit,
                    statusEffects: StatusEffectRequestFactory.FromPayloads(onHitEffects, attacker)
                );
        }

        public static DamageInfo FromMeleeWeapon
            (
                MeleeWeaponData weapon,
                GameObject attacker,
                Vector3 hitPoint,
                Vector3 hitNormal = default
            )
        {
            var hit = weapon.HitEffects;

            if (hitNormal == default)
                hitNormal = Vector3.up;

            return new DamageInfo
                (
                    amount: hit.Damage,
                    type: hit.DamageKind,
                    instigator: attacker,
                    hitPoint: hitPoint,
                    hitNormal: hitNormal,
                    stunDuration: hit.StunDuration,
                    knockbackForce: hit.KnockbackForce,
                    impulseType: hit.ImpulseKind,
                    source: DamageSource.DirectHit,
                    statusEffects: StatusEffectRequestFactory.FromPayloads(hit.StatusEffects, attacker)
                );
        }

        public static DamageInfo FromStatusTick
            (
                int amount,
                DamageType type,
                GameObject source,
                Vector3 targetPosition
            )
        {
            return new DamageInfo
                (
                    amount: amount,
                    type: type,
                    instigator: source,
                    hitPoint: targetPosition,
                    hitNormal: Vector3.up,
                    source: DamageSource.StatusTick
                );
        }

        public static DamageInfo FromEnvironment
            (
                int amount,
                DamageType type,
                Vector3 hitPoint,
                GameObject source = null
            )
        {
            return new DamageInfo
                (
                    amount: amount,
                    type: type,
                    instigator: source,
                    hitPoint: hitPoint,
                    hitNormal: Vector3.up,
                    source: DamageSource.Environment
                );
        }

        public static DamageInfo FromHazardZone
            (
                HazardZoneData zone,
                GameObject source,
                Vector3 hitPoint
            )
        {
            HitEffectProfile hit = zone.HitEffects;

            StatusEffectPayload[] statuses = zone.ApplyStatusEffects
                ? hit.StatusEffects
                : null;

            int amount = zone.DealHitDamage ? hit.Damage : 0;

            return new DamageInfo
                (
                    amount: amount,
                    type: hit.DamageKind,
                    instigator: source,
                    hitPoint: hitPoint,
                    hitNormal: Vector3.up,
                    stunDuration: zone.DealHitDamage ? hit.StunDuration : 0f,
                    knockbackForce: zone.DealHitDamage ? hit.KnockbackForce : 0f,
                    impulseType: hit.ImpulseKind,
                    source: DamageSource.Environment,
                    statusEffects: StatusEffectRequestFactory.FromPayloads(statuses, source)
                );
        }
    }
}
