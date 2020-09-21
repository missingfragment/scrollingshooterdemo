using System;
using UnityEngine;
using System.Collections;

namespace SpaceShooterDemo
{
    /// <summary>
    /// A class responsible for tracking a ship's health
    /// and triggering events when its HP changes or when it gets destroyed.
    /// </summary>
    public class SpaceShip : MonoBehaviour
    {
        // constants
        protected const int PLAYER_LAYER = 8;
        protected const int ENEMY_LAYER = 9;

        // events

        /// <summary>
        /// Whenever any SpaceShip gets destroyed.  Reports a score value.
        /// </summary>
        public static event EventHandler<ShipDestroyedEventArgs> Destroyed;
        /// <summary>
        /// Whenever this SpaceShip has its Health value changed.
        /// Reports the previous and new current value.
        /// </summary>
        public event EventHandler<ShipHealthChangedEventArgs> HealthChanged;

        // fields
        [SerializeField]
        protected Team alignment = default;

        [SerializeField]
        protected int scoreValue = default;

        [SerializeField]
        protected int maxHealth = default;

        // How much damage is caused to other (enemy) ships when colliding.
        [SerializeField]
        protected int bumpDamage = 1;

        [SerializeField]
        protected SpriteRenderer sprite = default;

        // TODO: Make Graphics class for managing sprites
        [SerializeField]
        protected SpriteRenderer shieldSprite = default;

        [SerializeField]
        protected Color damageFlashColor = Color.red;

        [SerializeField]
        protected float invincibilityDuration = 0f;

        private int damage;

        private Coroutine damageFlashCoroutine;
        private Coroutine invincibilityCoroutine;

        private readonly AnimatedValue<Color> damageFlash
            = new AnimatedValue<Color>(10f);

        public Team Alignment => alignment;

        // properties

        /// <summary>
        /// Automatically keeps HP clamped.
        /// Current Health is calculated by taking maxHealth
        /// and subtracting damage to find the current HP value.
        /// </summary>
        public int Health
        {
            get =>
                Mathf.Clamp(maxHealth - damage, 0, maxHealth);
            protected set =>
                damage = Mathf.Clamp(maxHealth - value, 0, maxHealth);
        }

        public int MaxHealth => maxHealth;

        public int BumpDamage => bumpDamage;

        public bool Invincible { get; set; } = false;

        // methods

        protected virtual void Awake()
        {
            ShipHealthChangedEventArgs args =
                new ShipHealthChangedEventArgs
                {
                    OldHealthValue = 0,
                    NewHealthValue = Health,
                };

            HealthChanged?.Invoke(this, args);
        }

        protected IEnumerator TemporaryInvincibility(float? duration = null)
        {
            Invincible = true;
            float timer;

            // null check
            if (duration is float dur)
            {
                timer = dur;
            }
            else
            {
                timer = invincibilityDuration;
            }    

            shieldSprite.gameObject.SetActive(true);

            shieldSprite.color = Color.cyan;

            yield return null;

            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                if (timer <= invincibilityDuration / 2)
                {
                    shieldSprite.color = new Color(1, 1, 1, .5f);
                }
                yield return null;
            }

            shieldSprite.gameObject.SetActive(false);
            Invincible = false;
        }
        /// <summary>
        /// Causes the ship to take damage.
        /// Destroys the ship if Health drops to 0 or below.
        /// </summary>
        /// <param name="amount">
        /// How much damage to take.
        /// Negative values are valid and function as healing.
        /// </param>
        public virtual void TakeDamage(int amount)
        {
            if ((Invincible && amount > 0) || amount == 0)
            {
                return;
            }

            AddHealth(-amount);

            // If amount is less than zero, it is healing.
            if (amount > 0)
            {
                if (damageFlashCoroutine != null)
                {
                    StopCoroutine(damageFlashCoroutine);
                }
                //damageFlashCoroutine = StartCoroutine(DamageFlash());

                damageFlashCoroutine = StartCoroutine(
                    damageFlash.Animate(
                        damageFlashColor,
                        Color.white,
                        Color.Lerp,
                        (Color color) => sprite.color = color
                        )
                    );

                if (invincibilityDuration > 0f)
                {
                    if (invincibilityCoroutine != null)
                    {
                        StopCoroutine(invincibilityCoroutine);
                    }
                    invincibilityCoroutine =
                        StartCoroutine(TemporaryInvincibility());
                }
            }


            if (Health <= 0)
            {
                Explode();
            }
        }

        protected void AddHealth(int amount)
        {
            ShipHealthChangedEventArgs args =
                new ShipHealthChangedEventArgs
                {
                    OldHealthValue = Health
                };

            Health += amount;

            args.NewHealthValue = Health;

            HealthChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Destroys the ship.
        /// Can be called externally for instant death.
        /// </summary>
        public virtual void Explode()
        {
            ShipDestroyedEventArgs args =
                new ShipDestroyedEventArgs
                {
                    Alignment = Alignment,
                    ScoreValue = scoreValue
                };

            Destroyed?.Invoke(this, args);

            Explosion explosion = ExplosionPool.Instance.Get();

            explosion.transform.position = transform.position;
            explosion.gameObject.SetActive(true);

            Remove();
        }

        /// <summary>
        /// Removes the ship from play.
        /// Does not create any effects.
        /// </summary>
        public virtual void Remove()
        {
            // TODO: replace with object pooling for enemies.
            Destroy(gameObject);
        }

        /// <summary>
        /// When this SpaceShip collides with another SpaceShip.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            SpaceShip other = collision.gameObject.GetComponent<SpaceShip>();

            if (other != null)
            {
                if ((Alignment == Team.Player && other.gameObject.layer == ENEMY_LAYER)
                    || (Alignment == Team.Enemy && other.gameObject.layer == PLAYER_LAYER))
                {
                    if (!Invincible)
                    {
                        TakeDamage(other.BumpDamage);
                    }
                }
            }
        }
    }

}
