using UnityEngine;
using System;

namespace SpaceShooterDemo
{
    public class PlasmaGun : Weapon
    {
        public override void Fire()
        {
            Debug.Log("fire");
            PlasmaBolt projectile = PlasmaBoltPool.Instance.Get();

            projectile.gameObject.SetActive(true);

            projectile.transform.position = transform.position;

            projectile.Mover.Velocity = mover.Velocity;

            Vector3 direction = transform.up;
            projectile.Init(power, alignment,
                new Vector2(direction.x, direction.y)
                );

            SetCooldownTimer(cooldownDuration);
        }
    }
}
