using UnityEngine;

namespace SpaceShooterDemo
{
    [RequireComponent(typeof(Movement))]
    public abstract class ShipControl : MonoBehaviour
    {
        protected Movement mover;

        protected bool firing = false;

        [SerializeField]
        private Weapon[] weapons = default;

        protected virtual void Awake()
        {
            mover = GetComponent<Movement>();
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
