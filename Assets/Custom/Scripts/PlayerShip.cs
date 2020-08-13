using Unity;

namespace SpaceShooterDemo
{
    public class PlayerShip : SpaceShip
    {
        public static PlayerShip Instance { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}
