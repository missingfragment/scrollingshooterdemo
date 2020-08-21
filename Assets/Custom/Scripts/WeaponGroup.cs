using UnityEngine;
using System.Collections;

namespace SpaceShooterDemo
{
    /// <summary>
    /// A class for holding references to Weapon objects
    /// and coordinating them to fire at the same time
    /// and with a single sound effect.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class WeaponGroup : MonoBehaviour
    {
        // fields

        [SerializeField]
        protected int volleyLength;

        [SerializeField]
        protected float reloadDuration;

        private float reloadTimer;
        private float volleyCount;

        protected Weapon[] weapons;
        protected AudioSource audioSource;

        // methods
        protected void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            weapons = GetComponentsInChildren<Weapon>();
        }

        private IEnumerator Reload()
        {
            reloadTimer = reloadDuration;

            yield return null;

            while(reloadTimer > 0f)
            {
                reloadTimer -= Time.deltaTime;
                yield return null;
            }
            reloadTimer = 0f;
        }

        /// <summary>
        /// Fires all of the weapons in this WeaponGroup.
        /// Waits for all weapons in the group to be ReadyToFire.
        /// </summary>
        public void Fire()
        {
            if (volleyLength != 0)
            {
                if (volleyCount > volleyLength)
                {
                    volleyCount = 0;
                    StartCoroutine(Reload());
                    return;
                }

                if (reloadTimer > 0f)
                {
                    return;
                }
            }

            bool readyToFire = true;

            for(var i = 0; i < weapons.Length; i++)
            {
                var weapon = weapons[i];
                if (!weapon.Enabled)
                {
                    continue;
                }

                readyToFire = readyToFire && weapon.ReadyToFire;
            }

            if (!readyToFire)
            {
                return;
            }

            if (volleyLength != 0)
            {
                volleyCount++;
            }

            audioSource.PlayOneShot(audioSource.clip);

            for (var i = 0; i < weapons.Length; i++)
            {
                var weapon = weapons[i];
                if (weapon.Enabled)
                {
                    weapon.Fire();
                }
            }
        }
    }
}
