using UnityEngine;
using System;

namespace SpaceShooterDemo
{
    public class PlasmaGun : Weapon
    {
        public override void Fire()
        {
            GetProjectileFromPool getProjectile = PlasmaBoltPool.Instance.Get;

            Vector3 direction = transform.up;
            Vector2 direction2 = new Vector2(direction.x, direction.y);

            Fire(getProjectile, direction2);
        }
    }
}
