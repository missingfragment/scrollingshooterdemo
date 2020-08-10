using UnityEngine;

namespace SpaceShooterDemo
{
    public class FloaterGun : Weapon
    {
        public override void Fire()
        {
            GetProjectileFromPool getProjectile = FloaterBulletPool.Instance.Get;

            PlayerShip player = PlayerShip.Instance;

            Vector2 direction2d;

            if (player != null)
            {
                Vector3 direction =
                    player.transform.position - transform.position;

                direction.Normalize();

                direction2d = new Vector2(direction.x, direction.y);
            }
            else
            {
                direction2d = Vector2.up;
            }


            Fire(getProjectile, direction2d);
        }
    }
}
