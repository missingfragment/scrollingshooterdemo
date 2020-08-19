using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        protected Weapon[] weapons;
        protected AudioSource audioSource;

        // methods
        protected void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            weapons = GetComponentsInChildren<Weapon>();
        }

        /// <summary>
        /// Fires all of the weapons in this WeaponGroup.
        /// Waits for all weapons in the group to be ReadyToFire.
        /// </summary>
        public void Fire()
        {
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
