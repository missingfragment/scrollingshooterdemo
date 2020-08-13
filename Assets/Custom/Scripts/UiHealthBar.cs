using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace SpaceShooterDemo
{
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
        }

        private void OnDestroy()
        {
            Unbind();
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
            StartCoroutine(
                GraduallyUpdateBarValue(
                    e.OldHealthValue,
                    e.NewHealthValue)
                );
        }

        private IEnumerator GraduallyUpdateBarValue(float oldValue,
            float newValue)
        {
            float progress = 0f;
            while (progress < 1f)
            {
                BarValue = Mathf.Lerp(oldValue, newValue, progress);
                UpdateBarFill();
                progress += barChangeSpeed * Time.deltaTime;
                yield return null;
            }
            BarValue = newValue;
            UpdateBarFill();
        }
    }
}
