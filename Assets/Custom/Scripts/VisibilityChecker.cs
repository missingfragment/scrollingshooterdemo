using UnityEngine;
using System;

namespace SpaceShooterDemo
{
    /// <summary>
    /// Provides access to the OnBecameInvisible() and
    /// OnBecameVisible() events to parents without graphic components.
    /// </summary>
    public class VisibilityChecker : MonoBehaviour
    {
        public event Action<bool> VisibilityChanged;

        private void OnBecameInvisible()
        {
            VisibilityChanged?.Invoke(false);
        }

        private void OnBecameVisible()
        {
            VisibilityChanged?.Invoke(true);
        }
    }
}
