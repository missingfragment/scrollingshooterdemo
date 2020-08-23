using UnityEngine;

namespace SpaceShooterDemo
{
    /// <summary>
    /// Manages thruster graphics.
    /// </summary>
    [RequireComponent(typeof(Movement))]
    public class Thruster : MonoBehaviour
    {
        [SerializeField]
        private GameObject smallThruster = default;
        [SerializeField]
        private GameObject largeThruster = default;
        [SerializeField]
        private ParticleSystem particles = default;

        private Movement mover;

        private void Start()
        {
            mover = GetComponent<Movement>();
        }

        private void Update()
        {
            //Vector2 velocity = mover.Inputs;
            Vector2 velocity = mover.Velocity;
            var minSpeed = .1f;
            if (velocity.y > minSpeed)
            {
                largeThruster.SetActive(true);
                smallThruster.SetActive(false);

                if (!particles.isPlaying)
                {
                    particles.Play();
                }
            }
            else if (velocity.y < -minSpeed)
            {
                largeThruster.SetActive(false);
                smallThruster.SetActive(false);

                particles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
            else
            {
                largeThruster.SetActive(false);
                smallThruster.SetActive(true);

                if (!particles.isPlaying)
                {
                    particles.Play();
                }
            }
        }
    }
}
