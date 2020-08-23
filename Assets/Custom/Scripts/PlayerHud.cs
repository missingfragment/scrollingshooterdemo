using UnityEngine;
using TMPro;

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

        [SerializeField]
        protected TMP_Text livesText = default;

        private Coroutine coroutine;

        private readonly AnimatedValue<float> animatedScore =
            new AnimatedValue<float>(SCORE_ANIM_SPEED);

        // methods

        // unity event methods
        private void OnEnable()
        {
            GameManager.ScoreChanged += OnScoreChanged;
            GameManager.LivesChanged += OnLivesChanged;
        }


        private void OnDisable()
        {
            GameManager.ScoreChanged -= OnScoreChanged;
            GameManager.LivesChanged -= OnLivesChanged;
        }

        // other methods

        private void OnLivesChanged(byte lives)
        {
            livesText.text = $"x{lives}";
        }

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
                    e.OldValue, e.NewValue,
                    Mathf.Lerp, UpdateScoreText
                    )
                );
        }
    }
}
