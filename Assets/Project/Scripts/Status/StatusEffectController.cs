using UnityEngine;
using Assets.Project.Scripts.Combat;
using System.Collections.Generic;

namespace Assets.Project.Scripts.Status
{
    [RequireComponent(typeof(DamageProcessor))]
    public class StatusEffectController : MonoBehaviour
    {
        private readonly List<ActiveStatusEffect> _activeEffects = new ();
        private DamageProcessor _damageProcessor;

        private void Awake()
        {
            _damageProcessor = GetComponent<DamageProcessor>();
        }
 
        private void Update()
        {
            for (int i = _activeEffects.Count - 1; i >=0; i--)
            {
                // Tick each active effect and check if it is still active
                bool stillActive = _activeEffects[i].Tick(Time.deltaTime, _damageProcessor);

                if (!stillActive)
                {
                    _activeEffects.RemoveAt(i);
                }
            }
        }

        public void ApplyStatusEffect(StatusEffectRequest request)
        {
            if (request.Duration <= 0) return; 

            // Stage 3: replace with a fresh instance of the effect
            for (int i = 0; i < _activeEffects.Count; i++)
            {
                if (_activeEffects[i].Type != request.Type) continue;

                // If the effect is already active, we can choose to refresh it or stack it based on your game design
                _activeEffects[i] = new ActiveStatusEffect(request);

                return;
            }

            // If the effect is not already active, add it to the list
            _activeEffects.Add(new ActiveStatusEffect(request));
        }

        public void ApplyAllStatusEffects(StatusEffectRequest[] requests)
        {
            if (requests == null || requests.Length == 0) return;

            foreach (var request in requests)
            {
                ApplyStatusEffect(request);
            }
        }

        public bool HasStatusEffect(StatusEffectType type)
        {
            for (int i = 0; i< _activeEffects.Count; i++)
            {
                if (_activeEffects[i].Type == type) return true;
            }

            return false;
        }

        public void ClearAllStatusEffects()
        {
            _activeEffects.Clear();
        }
    }
}
