using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooterDemo
{
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
            Vector2 velocity = mover.Inputs;
            if (velocity.y > 0)
            {
                largeThruster.SetActive(true);
                smallThruster.SetActive(false);

                if (!particles.isPlaying)
                {
                    particles.Play();
                }
            }
            else if (velocity.y < 0)
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
