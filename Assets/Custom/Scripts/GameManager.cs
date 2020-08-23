using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

        /// <summary>
        /// Whenever Lives changes.
        /// </summary>
        public static event Action<byte> LivesChanged;

        // constants
        private const int EXTRA_LIFE_BASE_THRESHOLD = 100000;
        private const float RESPAWN_DELAY = 1f;

        // fields
        private static int score;
        private static byte lives;

        private int extraLifeThreshold = EXTRA_LIFE_BASE_THRESHOLD;

        public bool Pause { get; private set; }

        // properties
        public static int Score
        {
            get => score;
            set
            {
                var eventArgs = new ScoreChangedEventArgs
                {
                    OldValue = score,
                    NewValue = value
                };

                score = value;
                ScoreChanged?.Invoke(null, eventArgs);
            }
        }

        public static byte Lives
        {
            get => lives;
            set
            {
                lives = value;
                LivesChanged?.Invoke(lives);
            }
        }

        private void Start()
        {
            Score = 0;
            Lives = 3;
        }

        private void OnEnable()
        {
            SpaceShip.Destroyed += OnShipDestroyed;
            ScoreChanged += OnScoreChanged;

            InputHandler.InputReceived += OnInput;
        }

        private void OnDisable()
        {
            SpaceShip.Destroyed -= OnShipDestroyed;
            ScoreChanged -= OnScoreChanged;

            InputHandler.InputReceived -= OnInput;
        }

        private void OnInput(InputAction.CallbackContext context)
        {
            if (context.action.name == "Pause")
            {
                Pause = !Pause;

                if (Pause)
                {
                    Time.timeScale = 0f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }
        }

        private void OnScoreChanged(object sender, ScoreChangedEventArgs e)
        {
            if (e.NewValue >= extraLifeThreshold)
            {
                extraLifeThreshold += EXTRA_LIFE_BASE_THRESHOLD;
                if (Lives < 100)
                {
                    Lives++;
                    AudioManager.Instance.Play("1up");
                }
            }
        }

        private IEnumerator RespawnPlayer()
        {
            yield return new WaitForSeconds(RESPAWN_DELAY);

            PlayerShip.Instance.Respawn();
        }

        private void OnShipDestroyed(object sender, ShipDestroyedEventArgs e)
        {
            if (sender is PlayerShip)
            {
                if (Lives > 0)
                {
                    Lives--;
                    StartCoroutine(RespawnPlayer());
                }
                return;
            }

            Score += e.ScoreValue;
        }
    }
}
