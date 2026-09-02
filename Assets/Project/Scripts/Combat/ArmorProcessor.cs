using UnityEngine;

namespace Assets.Project.Scripts.Combat
{
    [RequireComponent(typeof(ArmorData))]
    public class ArmorProcessor : MonoBehaviour
    {
        private ArmorData _armorData;

        // Methods

        private void Awake()
        {
            _armorData = GetComponent<ArmorData>();
        }

        ///<summary>
        /// Calculates damage absorbed by armor and reduces the armor pooo.
        /// Returns the remaining unmitigate damage that should be dealt to health
        ///</summary>
        public int MitigateDamage(DamageInfo info)
        {
            // If armor is empty or missing, all damage goes to health
            if (_armorData.IsDepleted)
            {
                return info.Amount;
            }

            // 1) Substract flat reduction treshold; e.g. low calliber bullet resistance
            int effectiveDamage = Mathf.Max(1, info.Amount - _armorData.FlatDamageReduction);

            // 2) Split damage absorbed by armor vs damage passing to HP
            int damageToArmor = Mathf.RoundToInt(effectiveDamage * _armorData.AbsorptionRate);
            int passingDamage = effectiveDamage - damageToArmor;

            // 3) Deduct from current armor pool
            if (_armorData.CurrentArmor >= damageToArmor)
            {
                _armorData.CurrentArmor -= damageToArmor;
            }
            else
            {
                // Armor broke on this hit; ramainidng absorbed amount goes into passing damage
                int leftoverDamage = damageToArmor - _armorData.CurrentArmor;
                passingDamage += leftoverDamage;
                _armorData.CurrentArmor = 0;
                _armorData.NotifyBroken();
            }

            _armorData.NotifyChanged();
            return passingDamage;
        }

        ///<summary>
        /// Restores armor points up to MaxArmor
        ///</summary>
        public void RepairArmor(int amount)
        {
            if (amount <= 0 || _armorData.IsFullArmor) return;

            _armorData.CurrentArmor = Mathf.Min(_armorData.MaxArmor, _armorData.CurrentArmor + amount);
            _armorData.NotifyChanged();
        }
    }
}