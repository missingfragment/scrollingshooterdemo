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

        protected SpaceShip boundSpaceShip;

        protected void OnEnable()
        {
            SpaceShip.Destroyed += OnSpaceShipDestroyed;
        }

        protected void OnDisable()
        {
            SpaceShip.Destroyed -= OnSpaceShipDestroyed;
        }

        protected void Start()
        {
            Bind(PlayerShip.Instance);
        }

        public void Bind(SpaceShip target)
        {
            if (target == null)
            {
                return;
            }

            if (boundSpaceShip != null)
            {
                Unbind();
            }

            boundSpaceShip = target;
            boundSpaceShip.HealthChanged += OnSpaceShipHealthChanged;

        }

        public void Unbind()
        {
            if (boundSpaceShip == null)
            {
                return;
            }
            boundSpaceShip.HealthChanged -= OnSpaceShipHealthChanged;
            boundSpaceShip = null;
            UpdateTextField(boundSpaceShip.MaxHealth);
        }

        private void UpdateTextField(int amount)
        {
            healthTextField.text = $"{HEALTH_PREFIX} {amount}";
        }

        private void OnSpaceShipHealthChanged(
            object sender,
            ShipHealthChangedEventArgs e)
        {
            UpdateTextField(e.NewHealthValue);
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
