using UnityEngine;

namespace SpaceShooterDemo
{
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(SpaceShip))]
    public abstract class ShipControl : MonoBehaviour
    {
        protected Movement mover;

        protected bool firing = false;

        private WeaponGroup[] weaponGroups;

        protected VisibilityChecker visibilityChecker;
        protected SpaceShip spaceShip;

        protected virtual void Awake()
        {
            mover = GetComponent<Movement>();
            spaceShip = GetComponent<SpaceShip>();
            visibilityChecker = GetComponentInChildren<VisibilityChecker>();

            weaponGroups = GetComponentsInChildren<WeaponGroup>();
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
            for (var i = 0; i < weaponGroups.Length; i++)
            {
                weaponGroups[i].Fire();
            }
        }
    }
}
