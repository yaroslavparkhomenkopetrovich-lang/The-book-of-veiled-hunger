using UnityEngine;

namespace VeiledHunger.Core
{ 
    public interface IDamageable
    {
        void TakeDamage(int damageAmount, Vector3 hitPoint, Vector3 hitNormal);
    }
}
