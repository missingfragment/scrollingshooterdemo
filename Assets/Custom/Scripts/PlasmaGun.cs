using UnityEngine;
using System;

namespace SpaceShooterDemo
{
    public class PlasmaGun : Weapon
    {
        public override void Fire()
        {
            // Pass the appropriate function into the base class'
            // Fire() overload function so that it knows how to
            // get an appropriate projectile.
            // Doing this lets us avoid copying code for setting up
            // the projectile into every class.
            GetProjectileFromPool getProjectile = PlasmaBoltPool.Instance.Get;

            Fire(getProjectile, Vector2.up);
        }
    }
}
