using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooterDemo
{
    /// <summary>
    /// Manages the game state and holds meta values like the Score.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        // events
        /// <summary>
        /// Whenever the Score changes.
        /// </summary>
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
