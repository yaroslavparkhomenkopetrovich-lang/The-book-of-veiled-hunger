namespace Assets.Project.Scripts.Combat
{
    public enum ImpulseType
    {
        PushAwayFromBullet, // Standard: pushes along the projectile's flight direction
        PushAwayFromImpact, // Retro: pushes away from the hit point (like a shotgun blast)
        PullTowardImpact,   // Vortex: pulls toward the impact center
        PullTowardShooter   // Harpoon: pulls enemies toward the player
    }
}
