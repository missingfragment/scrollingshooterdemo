using Unity;

namespace SpaceShooterDemo
{
    public class PlayerShip : SpaceShip
    {
        public static PlayerShip Instance { get; private set; }

        protected override void Start()
        {
            base.Start();

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
