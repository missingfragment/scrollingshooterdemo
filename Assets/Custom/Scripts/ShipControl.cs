using UnityEngine;

namespace SpaceShooterDemo
{
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(ShipControl))]
    public abstract class ShipControl : MonoBehaviour
    {
        protected Movement mover;

        protected bool firing = false;

        [SerializeField]
        private Weapon[] weapons = default;

        protected VisibilityChecker visibilityChecker;
        protected SpaceShip spaceShip;

        protected virtual void Awake()
        {
            mover = GetComponent<Movement>();
            spaceShip = GetComponent<SpaceShip>();
            visibilityChecker = GetComponentInChildren<VisibilityChecker>();
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
                }
            }
        }
    }
}
