using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooterDemo
{
    public abstract class Weapon : MonoBehaviour, IWeapon
    {
        // delegates

        // A delegate that stores a function for retrieving
        // a Projectile from an ObjectPool.
        protected delegate Projectile GetProjectileFromPool();

        // fields

        private float cooldownTimer = 0f;

        protected Movement mover;

        protected Team alignment = default;

        [SerializeField]
        protected int power = default;

        [SerializeField]
        protected float cooldownDuration = default;

        [SerializeField]
        protected string soundEffectId = default;

        // properties
        public Transform Transform => transform;

        public bool Enabled { get; set; } = true;

        public bool ReadyToFire => cooldownTimer <= 0f;

        public Team Alignment => alignment;

        public string SoundEffectId => soundEffectId;

        // methods

        protected virtual void Awake()
        {
            mover = GetComponentInParent<Movement>();
            SpaceShip spaceShip = GetComponentInParent<SpaceShip>();
            alignment = spaceShip.Alignment;
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

        // public overload from IWweapon, to be implemented by children classes
        public abstract void Fire();

        // protected overload to be called in child implementations
        // Takes a delegate that supplies it with the proper function
        // for retrieving a projectile from its respective pool.
        protected void Fire(GetProjectileFromPool getProjectile,
            Vector2 direction)
        {
            if (getProjectile == null)
            {
                return;
            }

            Projectile projectile = getProjectile();

            projectile.transform.position = transform.position;
            projectile.transform.rotation = transform.rotation;

            //projectile.Mover.Velocity = mover.Velocity;

            projectile.Init(power, alignment, direction);

            projectile.gameObject.SetActive(true);

            SetCooldownTimer(cooldownDuration);
        }
    }
}
