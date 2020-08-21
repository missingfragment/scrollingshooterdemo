using UnityEngine;

namespace SpaceShooterDemo
{
    /// <summary>
    /// A Weapon is responsible for shooting Projectiles.
    /// Implements the IWeapon interface.
    /// </summary>
    public abstract class Weapon : MonoBehaviour, IWeapon
    {
        // delegates

        /// <summary>
        /// A delegate that stores a function for retrieving
        /// a Projectile from an ObjectPool.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// A public overload from IWeapon,
        /// to be implemented by child classes
        /// </summary>
        public abstract void Fire();

        /// <summary>
        /// Fires a projectile.
        /// This overload gives child classes a base implementation
        /// to use when implementing the public void Fire() method.
        /// </summary>
        /// <param name="getProjectile">
        /// A delegate that represents the proper function
        /// for retrieving a projectile from its respective pool.
        /// </param>
        /// <param name="direction">
        /// The Vector2 direction that the Projectile will fire.
        /// </param>
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
