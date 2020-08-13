using UnityEngine;
using TMPro;

namespace SpaceShooterDemo
{
    public class PlayerHud : MonoBehaviour
    {

        private const string HEALTH_PREFIX = "Health: ";
        private const string SHIP_DESTROYED = "SHIP DESTROYED";

        [SerializeField]
        protected TMP_Text healthTextField = default;

        protected void OnEnable()
        {
            SpaceShip.Destroyed += OnSpaceShipDestroyed;
            SpaceShip.HealthChanged += OnSpaceShipHealthChanged;
        }

        protected void OnDisable()
        {
            SpaceShip.Destroyed -= OnSpaceShipDestroyed;
            SpaceShip.HealthChanged -= OnSpaceShipHealthChanged;
        }

        private void OnSpaceShipHealthChanged(
            object sender,
            ShipHealthChangedEventArgs e)
        {
            if (sender is PlayerShip)
            {
                healthTextField.text = $"{HEALTH_PREFIX} {e.NewHealthValue}";
            }
        }

        private void OnSpaceShipDestroyed(
            object sender,
            ShipDestroyedEventArgs e)
        {
            if (sender is PlayerShip)
            {
                healthTextField.text = $"{SHIP_DESTROYED}";
            }
        }
    }
}
