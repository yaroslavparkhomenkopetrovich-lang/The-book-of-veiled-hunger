using UnityEngine;

namespace Assets.Project.Scripts.Status
{
    public enum StatusEffectType
    {
        Burn,
        Bleed,
        Stun,       // Long entity status (not the same as tiny hit stun)
        Slow,
        ArmorDecay
    }
}
