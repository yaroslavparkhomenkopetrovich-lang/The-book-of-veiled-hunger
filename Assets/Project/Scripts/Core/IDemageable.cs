using UnityEngine;

public interface IDemageable
{
    void TakeDamage(int damageAmoubt, Vector3 hitPoint, Vector3 hitNormal);
}
