using UnityEngine;

namespace SpaceShooterDemo
{
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(SpaceShip))]
    public abstract class ShipControl : MonoBehaviour
    {
        protected Movement mover;

        protected bool firing = false;

        [SerializeField]
        private Weapon[] weapons = default;

        protected VisibilityChecker visibilityChecker;
        protected SpaceShip spaceShip;

        protected AudioSource weaponSound;

        protected virtual void Awake()
        {
            mover = GetComponent<Movement>();
            spaceShip = GetComponent<SpaceShip>();
            visibilityChecker = GetComponentInChildren<VisibilityChecker>();
            weaponSound = GetComponent<AudioSource>();
        }

        protected virtual void Update()
        {
            if (firing)
            {
                Fire();
            }
        }

        protected virtual void Fire()
        {
            for (var i = 0; i < weapons.Length; i++)
            {
                var weapon = weapons[i];
                if (weapon.Enabled && weapon.ReadyToFire)
                {
                    weapon.Fire();
                    if (weaponSound != null)
                    {
                        weaponSound.PlayOneShot(weaponSound.clip, 1.0f);
                    }
                }
            }
        }
    }
}
