using UnityEngine;
using System.Collections;

namespace SpaceShooterDemo
{
    /// <summary>
    /// A SpaceShip that is controlled by the player.
    /// </summary>
    public class PlayerShip : SpaceShip
    {
        [SerializeField]
        protected float hpRechargeDuration = 2f;

        [SerializeField]
        protected float respawnInvincibilityDuration = default;

        [SerializeField]
        protected SpriteRenderer dangerSprite = default;

        public static PlayerShip Instance { get; private set; }

        private Coroutine hpRechargeCoroutine;

        protected override void Awake()
        {
            base.Awake();

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public override void Remove()
        {
            gameObject.SetActive(false);
            dangerSprite.gameObject.SetActive(false);
            sprite.color = Color.white;
            transform.position = new Vector3(-100, -100, -100);
        }

        public void Respawn()
        {
            gameObject.SetActive(true);

            Health = 0;
            AddHealth(maxHealth);

            transform.position = new Vector3(0, -1);
            StartCoroutine(
                TemporaryInvincibility(respawnInvincibilityDuration)
                );

            AudioManager.Instance.Play("recover");
        }

        public override void TakeDamage(int amount)
        {
            if (amount == 0)
            {
                return;
            }

            if (Health <= 1)
            {
                base.TakeDamage(amount);
                if (hpRechargeCoroutine != null)
                {
                    StopCoroutine(hpRechargeCoroutine);
                }
                AudioManager.Instance.Stop("alarm");
            }
            else
            {
                // Jump hp down to 1, and recharge after a few seconds.
                int newHp = Mathf.Clamp(Health - amount, 0, MaxHealth);
                base.TakeDamage(Health - 1);
                hpRechargeCoroutine = StartCoroutine(HpRecharge(newHp));
                AudioManager.Instance.Play("damage");
                AudioManager.Instance.Play("alarm");
            }
        }

        /// <summary>
        /// A coroutine that drops the PlayerShip's HP to 1
        /// and then refills it after a set duration.
        /// </summary>
        /// <param name="finalHpAmount">
        /// The HP value to restore the ship to after the duration.
        /// </param>
        /// <returns>
        /// A Coroutine.
        /// </returns>
        private IEnumerator HpRecharge(int finalHpAmount)
        {
            dangerSprite.gameObject.SetActive(true);
            yield return new WaitForSeconds(hpRechargeDuration);
            // Restore health by the new hp minus 1 (since we are at 1 hp)
            TakeDamage(-(finalHpAmount - 1));
            if (Health > 1)
            {
                dangerSprite.gameObject.SetActive(false);
                AudioManager.Instance.Play("recover");
            }
        }
    }

}
