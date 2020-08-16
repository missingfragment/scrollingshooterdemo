using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooterDemo
{
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
