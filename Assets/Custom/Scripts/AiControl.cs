using System;
using System.Collections;
using UnityEngine;

namespace SpaceShooterDemo
{
    public class AiControl : ShipControl
    {
        [SerializeField]
        private float entryTime = default;
        [SerializeField]
        private float stationaryTime = default;

        protected void Start()
        {
            StartCoroutine(RunAi());
        }

        protected void OnEnable()
        {
            visibilityChecker.VisibilityChanged += OnVisibilityChanged;
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

            if (!visible)
            {
                spaceShip.Remove();
            }
        }

        protected virtual IEnumerator RunAi()
        {
            // Enter the camera's view
            mover.Inputs = Vector2.up;
            yield return new WaitForSeconds(entryTime);

            // Stop moving and start shooting
            mover.Inputs = Vector2.zero;
            firing = true;
            yield return new WaitForSeconds(stationaryTime);

            // Resume moving
            mover.Inputs = Vector2.up;
            yield return null;
        }
    }
}
