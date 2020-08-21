using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceShooterDemo
{
    /// <summary>
    /// Provides the player an interface to control a PlayerShip.
    /// </summary>
    public class PlayerControl : ShipControl
    {
        protected void OnEnable()
        {
            InputHandler.InputReceived += OnInput;
        }

        protected void OnDisable()
        {
            InputHandler.InputReceived -= OnInput;

            firing = false;
        }

        private void OnInput(InputAction.CallbackContext context)
        {
            switch(context.action.name)
            {
                case "Fire":
                    OnFireInput(context);
                    break;
                case "Move":
                    OnMoveInput(context);
                    break;
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
    }
}
