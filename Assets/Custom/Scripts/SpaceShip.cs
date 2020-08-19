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
        protected const int PlayerLayer = 8;
        protected const int EnemyLayer = 9;

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

        private IEnumerator DamageFlash()
        {
            sprite.color = damageFlashColor;
            //yield return new WaitForSeconds(.2f);
            float progress = 0f;

            float flashSpeed = 1.5f;

            Color startColor = sprite.color;

            while (progress < 1f)
            {
                sprite.color = Color.Lerp(startColor, Color.white,
                    progress);
                progress += flashSpeed * Time.deltaTime;
                yield return null;
            }
            sprite.color = Color.white;
        }

        protected IEnumerator TemporaryInvincibility()
        {
            Invincible = true;
            var timer = invincibilityDuration;

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

            ShipHealthChangedEventArgs args =
                new ShipHealthChangedEventArgs
                {
                    OldHealthValue = Health
                };

            Health -= amount;

            args.NewHealthValue = Health;

            // If amount is less than zero, it is healing.
            if (amount > 0)
            {
                StartCoroutine(DamageFlash());

                if (invincibilityDuration > 0f)
                {
                    StartCoroutine(TemporaryInvincibility());
                }
            }

            HealthChanged?.Invoke(this, args);

            if (Health <= 0)
            {
                Explode();
            }
        }

        /// <summary>
        /// Destroys the ship.
        /// Can be called externally for instant death.
        /// </summary>
        public void Explode()
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
                if ((Alignment == Team.Player && other.gameObject.layer == EnemyLayer)
                    || (Alignment == Team.Enemy && other.gameObject.layer == PlayerLayer))
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
