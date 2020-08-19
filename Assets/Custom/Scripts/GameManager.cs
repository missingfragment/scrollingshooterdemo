using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooterDemo
{
    public class GameManager : MonoBehaviour
    {
        // events
        public static event EventHandler<ScoreChangedEventArgs> ScoreChanged;

        // fields
        private int _score;

        // properties
        public int Score
        {
            get => _score;
            set
            {
                var eventArgs = new ScoreChangedEventArgs
                {
                    OldValue = _score,
                    NewValue = value
                };

                _score = value;
                ScoreChanged?.Invoke(this, eventArgs);
            }
        }

        private void Start()
        {
            Score = 0;
        }

        private void OnEnable()
        {
            SpaceShip.Destroyed += OnShipDestroyed;
        }

        private void OnDisable()
        {
            SpaceShip.Destroyed -= OnShipDestroyed;
        }

        private void OnShipDestroyed(object sender, ShipDestroyedEventArgs e)
        {
            if (sender is PlayerShip)
            {
                return;
            }

            Score += e.ScoreValue;
        }
    }
}
