using UnityEngine;

namespace Assets.Project.Scripts.Combat
{
    public readonly struct DamageInfo
    {
        public readonly int Amount;                     // Raw damage value
        public readonly DamageType Type;                // Elemental, psysical, etc
        public readonly GameObject Instigator;          // Atacker for kills life steal
        public readonly Vector3 HitPoint;               // World position for hit effects
        public readonly Vector3 HitNormal;              // Surface normal for decal alignment

        public DamageInfo (int amount, DamageType type, GameObject instigator, Vector3 hitPoint, Vector3 hitNormal)
        {
            Amount = amount;
            Type = type;
            Instigator = instigator;
            HitPoint = hitPoint;
            HitNormal = hitNormal;
        }
    }
}
