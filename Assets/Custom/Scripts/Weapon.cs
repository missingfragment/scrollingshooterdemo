using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooterDemo
{
    public abstract class Weapon : MonoBehaviour, IWeapon
    {
        // fields
        private float cooldownTimer = 0f;

        protected Movement mover;

        [SerializeField]
        protected Team alignment = default;

        [SerializeField]
        protected int power = default;

        [SerializeField]
        protected float cooldownDuration = default;

        // properties
        public Transform Transform => transform;

        public bool Enabled { get; set; } = true;

        public bool ReadyToFire => cooldownTimer <= 0f;

        public Team Alignment => alignment; 

        // methods

        protected virtual void Awake()
        {
            mover = GetComponentInParent<Movement>();
        }

        protected virtual void Update()
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
        }

        protected void SetCooldownTimer(float value)
        {
            cooldownTimer = value;
        }

        public abstract void Fire();
    }
}
