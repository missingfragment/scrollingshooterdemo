using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceShooterDemo
{
    [RequireComponent(typeof(Movement))]
    public class PlayerControl : MonoBehaviour
    {
        private Movement mover;

        private bool firing = false;

        [SerializeField]
        private Weapon[] weapons = default;

        protected void Awake()
        {
            mover = GetComponent<Movement>();
        }

        protected void OnEnable()
        {
            InputHandler.MoveEvent += OnMoveInput;
            InputHandler.FireEvent += OnFireInput;
        }

        protected void OnDisable()
        {
            InputHandler.MoveEvent -= OnMoveInput;
            InputHandler.FireEvent -= OnFireInput;
        }

        private void Update()
        {
            if (firing)
            {
                Fire();
            }
        }

        private void OnMoveInput(InputAction.CallbackContext context)
        {
            Vector2 inputs = context.ReadValue<Vector2>();

            mover.Inputs = inputs;
        }

        private void OnFireInput(InputAction.CallbackContext context)
        {
            firing = !context.canceled;
        }

        private void Fire()
        {
            for (var i = 0; i < weapons.Length; i++)
            {
                var weapon = weapons[i];
                if (weapon.Enabled && weapon.ReadyToFire)
                {
                    weapon.Fire();
                }
            }
        }
    }
}
