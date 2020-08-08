using UnityEngine;
using System;

namespace SpaceShooterDemo
{
    public class PlasmaGun : Weapon
    {
        public override void Fire()
        {
            PlasmaBolt projectile = PlasmaBoltPool.Instance.Get();


            projectile.transform.position = transform.position;

            projectile.Mover.Velocity = mover.Velocity;

            Vector3 direction = transform.up;
            projectile.Init(power, alignment,
                new Vector2(direction.x, direction.y)
                );

            projectile.gameObject.SetActive(true);

            SetCooldownTimer(cooldownDuration);
        }
    }
}
