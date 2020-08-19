using UnityEngine;

namespace SpaceShooterDemo
{
    /// <summary>
    /// Responsible for controlling a SpaceShip's movement and weapons.
    /// </summary>
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

        /// <summary>
        /// Fires all of the WeaponGroups attached to this GameObject.
        /// </summary>
        protected virtual void Fire()
        {
            for (var i = 0; i < weaponGroups.Length; i++)
            {
                weaponGroups[i].Fire();
            }
        }
    }
}
