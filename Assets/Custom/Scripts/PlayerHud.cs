using UnityEngine;
using TMPro;
using System.Collections;

namespace SpaceShooterDemo
{
    /// <summary>
    /// A class responsible for updating HUD-related UI elements
    /// in response to changes in the game state.
    /// </summary>
    public class PlayerHud : MonoBehaviour
    {
        // constants
        private const int SCORE_ANIM_SPEED = 2;

        // fields
        [SerializeField]
        protected TMP_Text scoreText = default;

        private Coroutine coroutine;

        private AnimatedValue<float> animatedScore = new AnimatedValue<float>();

        // methods

        // unity event methods
        private void OnEnable()
        {
            GameManager.ScoreChanged += OnScoreChanged;
        }

        private void OnDisable()
        {
            GameManager.ScoreChanged -= OnScoreChanged;
        }

        // other methods

        private void UpdateScoreText(int score)
        {
            scoreText.text = score.ToString("N0");
        }

        private void UpdateScoreText(float score)
        {
            UpdateScoreText((int)score);
        }

        private void OnScoreChanged(object sender, ScoreChangedEventArgs e)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            //coroutine = StartCoroutine(AnimateScore(e.OldValue, e.NewValue));

            coroutine = StartCoroutine(
                animatedScore.Animate(
                    (float)e.OldValue, (float)e.NewValue,
                    Mathf.Lerp, UpdateScoreText
                    )
                );
        }
    }
}
