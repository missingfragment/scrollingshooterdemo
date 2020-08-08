using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceShooterDemo
{
    public class PlayerControl : ShipControl
    {
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

        private void OnMoveInput(InputAction.CallbackContext context)
        {
            Vector2 inputs = context.ReadValue<Vector2>();

            mover.Inputs = inputs;
        }

        private void OnFireInput(InputAction.CallbackContext context)
        {
            firing = !context.canceled;
        }
    }
}
