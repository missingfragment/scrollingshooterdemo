using UnityEngine;
using System;

namespace SpaceShooterDemo
{
    /// <summary>
    /// A Weapon that shoots PlasmaBolt Projectiles.
    /// </summary>
    public class PlasmaGun : Weapon
    {
        /// <summary>
        /// Fires a PlasmaBolt.
        /// </summary>
        public override void Fire()
        {
            GetProjectileFromPool getProjectile = PlasmaBoltPool.Instance.Get;

            Vector3 direction = transform.up;
            Vector2 direction2 = new Vector2(direction.x, direction.y);

            Fire(getProjectile, direction2);
        }
    }
}
