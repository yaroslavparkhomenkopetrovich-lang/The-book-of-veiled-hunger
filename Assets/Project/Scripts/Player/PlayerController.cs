using UnityEngine;
using Assets.Project.Scripts.Weapons;

namespace Assets.Project.Scripts.Player
{
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerWeaponController))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerInputHandler _inputHandler;
        private PlayerMovement _movement;
        private PlayerWeaponController _weaponController;

        private void Awake()
        {
            _inputHandler = GetComponent<PlayerInputHandler>();
            _movement = GetComponent<PlayerMovement>();
            _weaponController = GetComponent<PlayerWeaponController>();
        }  

        // Update is called once per frame
        void Update()
        {
            // 1) Send movement and aiming to PlayerMovement
            _movement.MoveTowards(_inputHandler.MoveInput);
            _movement.RotateTowards(_inputHandler.AimWorldPosition);

            // 2) Trigger firing in PlayerWeaponController
            if (_inputHandler.IsFirePressed)
            {
                _weaponController.TryFire();
            }
        }
    }
}