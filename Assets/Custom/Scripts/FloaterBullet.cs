namespace SpaceShooterDemo
{
    /// <summary>
    /// A Projectile that moves slowly in a set direction.
    /// </summary>
    public class FloaterBullet : Projectile
    {
        public override void Remove()
        {
            FloaterBulletPool.Instance.ReturnToPool(this);
        }
    }
}
