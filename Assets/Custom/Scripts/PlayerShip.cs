using UnityEngine;
using System.Collections;

namespace SpaceShooterDemo
{
    public class PlayerShip : SpaceShip
    {
        [SerializeField]
        protected float hpRechargeDuration = 2f;

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

        public override void TakeDamage(int amount)
        {
            if (Health <= 1)
            {
                base.TakeDamage(amount);
                if (hpRechargeCoroutine != null)
                {
                    StopCoroutine(hpRechargeCoroutine);
                }
            }
            else
            {
                // Jump hp down to 1, and recharge after a few seconds.
                int newHp = Mathf.Clamp(Health - amount, 0, MaxHealth);
                base.TakeDamage(Health - 1);
                hpRechargeCoroutine = StartCoroutine(HpRecharge(newHp));
            }
        }

        private IEnumerator HpRecharge(int finalHpAmount)
        {
            dangerSprite.gameObject.SetActive(true);
            yield return new WaitForSeconds(hpRechargeDuration);
            // Restore health by the new hp minus 1 (since we are at 1 hp)
            TakeDamage(-(finalHpAmount - 1));
            if (Health > 1)
            {
                dangerSprite.gameObject.SetActive(false);
            }
        }
    }

}
