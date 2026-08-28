namespace Assets.Project.Scripts.Combat
{
    public interface IArmor
    {
        // Return final damage after armor mitigation
        int MitigateDamage(DamageInfo damageInfo);

        // Current armor values for UI display
        int CurrentArmor { get; }
        int MaxArmor { get; }
    }
}
