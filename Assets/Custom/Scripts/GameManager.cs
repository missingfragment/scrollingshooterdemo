﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

        public static GameManager Instance { get; private set; }

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

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);

                return;
            }

            Instance = this;
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

        private IEnumerator LoadScene(string scene)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        public void StartGame()
        {
            StartCoroutine(LoadScene("main"));
        }

        public void QuitGame()
        {
            Application.Quit();
            Debug.Log("Exiting game.");
        }

        private void OnInput(InputAction.CallbackContext context)
        {
            if (context.action.name == "Pause")
            {
                Pause = !Pause;

                Time.timeScale = Pause ? 0f : 1f;
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
