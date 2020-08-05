using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Thruster : MonoBehaviour
{
    [SerializeField]
    private GameObject _smallThruster = default;
    [SerializeField]
    private GameObject _largeThruster = default;
    [SerializeField]
    private ParticleSystem _particleSystem = default;

    private Movement _mover;

    private void Start()
    {
        _mover = GetComponent<Movement>();
    }

    private void Update()
    {
        Vector2 velocity = _mover.Inputs;
        if (velocity.y > 0)
        {
            _largeThruster.SetActive(true);
            _smallThruster.SetActive(false);

            if (!_particleSystem.isPlaying)
            {
                _particleSystem.Play();
            }
        }
        else if (velocity.y < 0)
        {
            _largeThruster.SetActive(false);
            _smallThruster.SetActive(false);

            _particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }
        else
        {
            _largeThruster.SetActive(false);
            _smallThruster.SetActive(true);

            if (!_particleSystem.isPlaying)
            {
                _particleSystem.Play();
            }
        }
    }
}
