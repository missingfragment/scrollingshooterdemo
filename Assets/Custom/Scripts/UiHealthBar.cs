using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooterDemo
{
    /// <summary>
    /// Manages and animates a health bar graphic
    /// in response to player HP changes.
    /// </summary>
    public class UiHealthBar : MonoBehaviour
    {
        // fields
        [SerializeField]
        protected Image barImage;

        [SerializeField]
        protected Image dangerBarImage;

        [SerializeField]
        protected float maxValue = default;

        [SerializeField]
        protected float barValue = default;

        [SerializeField]
        protected float barChangeSpeed = 1;

        [SerializeField]
        protected float dangerThreshold = 0.1f;

        protected SpaceShip boundSpaceShip;

        protected AnimatedValue<float> animatedValue;
        private Coroutine coroutine;

        // properties

        public float BarFill
        {
            get => barValue / maxValue;
            set => barValue = value * maxValue;
        }

        public float BarValue
        {
            get => barValue;
            set => barValue = Mathf.Clamp(value, 0f, maxValue);
        }

        // methods

        private void UpdateBarFill()
        {
            SetBarFill(BarFill);
        }

        private void Start()
        {
            Bind(PlayerShip.Instance);
            animatedValue = new AnimatedValue<float>(barChangeSpeed);
        }

        private void OnDestroy()
        {
            Unbind();
        }

        private void SetBarValue(float amount)
        {
            BarValue = amount;
            UpdateBarFill();
        }

        private void SetBarFill(float amount)
        {
            if (amount <= dangerThreshold)
            {
                barImage.gameObject.SetActive(false);
                dangerBarImage.gameObject.SetActive(true);
            }
            else
            {
                barImage.gameObject.SetActive(true);
                dangerBarImage.gameObject.SetActive(false);
                barImage.fillAmount = amount;
            }
        }

        public void Bind(SpaceShip target)
        {
            if (target == null)
            {
                return;
            }

            boundSpaceShip = target;
            maxValue = boundSpaceShip.MaxHealth;
            boundSpaceShip.HealthChanged += OnShipHealthChanged;
            BarFill = 1f;
            UpdateBarFill();
        }

        public void Unbind()
        {
            if (boundSpaceShip == null)
            {
                return;
            }

            boundSpaceShip.HealthChanged -= OnShipHealthChanged;
            boundSpaceShip = null;
        }

        private void OnShipHealthChanged(object sender,
            ShipHealthChangedEventArgs e)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(
                animatedValue.Animate(
                    e.OldHealthValue,
                    e.NewHealthValue,
                    Mathf.Lerp,
                    SetBarValue
                    )
                );
        }
    }
}
