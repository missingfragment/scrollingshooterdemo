namespace SpaceShooterDemo
{
    public class FloaterGun : Weapon
    {
        public override void Fire()
        {
            GetProjectileFromPool getProjectile = FloaterBulletPool.Instance.Get;

            Fire(getProjectile);
        }
    }
}
