using System;
using System.Collections;
using UnityEngine;

namespace SpaceShooterDemo
{
    public class AiControl : ShipControl
    {
        [SerializeField]
        protected float entryTime = default;
        [SerializeField]
        protected float activeTime = default;

        protected void OnEnable()
        {
            visibilityChecker.VisibilityChanged += OnVisibilityChanged;
            StartCoroutine(RunAi());
        }

        protected void OnDisable()
        {
            visibilityChecker.VisibilityChanged -= OnVisibilityChanged;
        }

        private void OnVisibilityChanged(bool visible)
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            if (visible)
            {
                spaceShip.Invincible = false;
            }
            else
            {
                spaceShip.Remove();
            }
        }

        protected virtual IEnumerator RunAi()
        {
            // Enter the camera's view
            spaceShip.Invincible = true;
            mover.Inputs = Vector2.up;
            yield return new WaitForSeconds(entryTime);

            // Stop moving and start shooting
            mover.Inputs = Vector2.zero;
            firing = true;
            yield return new WaitForSeconds(activeTime);

            // Resume moving
            mover.Inputs = Vector2.up;
            yield return null;
        }
    }
}
