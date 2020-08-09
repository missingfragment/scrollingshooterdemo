namespace SpaceShooterDemo
{
    public class FloaterBullet : Projectile
    {
        public override void Remove()
        {
            FloaterBulletPool.Instance.ReturnToPool(this);
        }
    }
}
