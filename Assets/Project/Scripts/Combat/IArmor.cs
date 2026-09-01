namespace Assets.Project.Scripts.Combat
{
    public interface IArmor
    {
        // Return final damage after armor mitigation
        int MitigateDamage(DamageInfo damageInfo);
        void RepairArmor(int amount);

        // Current armor values for UI display
        int CurrentArmor { get; }
        int MaxArmor { get; }
    }
}
