using System;
using UnityEngine;

namespace SpaceShooterDemo
{
    public class ShipDestroyedEventArgs : EventArgs
    {
        public Team Alignment { get; set; }
        public int ScoreValue { get; set; }
    }

    public class ShipHealthChangedEventArgs : EventArgs
    {
        public int OldHealthValue { get; set; }
        public int NewHealthValue { get; set; }
    }

    public class SpaceShip : MonoBehaviour
    {
        // constants
        protected const int PlayerLayer = 8;
        protected const int EnemyLayer = 9;

        // events

        public event EventHandler<ShipDestroyedEventArgs> Destroyed;
        public event EventHandler<ShipHealthChangedEventArgs> HealthChanged;

        // fields
        [SerializeField]
        protected Team alignment = default;

        [SerializeField]
        protected int scoreValue = default;

        [SerializeField]
        protected int maxHealth = default;

        private int damage;

        public Team Alignment => alignment;

        // properties

        public int Health
        {
            get =>
                Mathf.Clamp(maxHealth - damage, 0, maxHealth);
            protected set =>
                damage = Mathf.Clamp(maxHealth - value, 0, maxHealth);
        }

        // methods
        public void TakeDamage(int amount)
        {
            ShipHealthChangedEventArgs args =
                new ShipHealthChangedEventArgs
                {
                    OldHealthValue = Health
                };

            Health -= amount;

            args.NewHealthValue = Health;

            HealthChanged?.Invoke(this, args);

            if (Health <= 0)
            {
                Explode();
            }
        }

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

        private void Remove()
        {
            // TODO: replace with object pooling for enemies.
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            GameObject other = collision.gameObject;
            if (Alignment == Team.Player && other.layer == EnemyLayer)
            {
                Explode();
            }
        }
    }

}
