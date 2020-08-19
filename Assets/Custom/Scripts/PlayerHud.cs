using UnityEngine;
using TMPro;
using System.Collections;

namespace SpaceShooterDemo
{
    public class PlayerHud : MonoBehaviour
    {
        private const int SCORE_ANIM_SPEED = 2;
        [SerializeField]
        protected TMP_Text scoreText = default;

        private Coroutine coroutine;

        private void OnEnable()
        {
            GameManager.ScoreChanged += OnScoreChanged;
        }

        private void OnDisable()
        {
            GameManager.ScoreChanged -= OnScoreChanged;
        }

        private IEnumerator AnimateScore(int startValue, int endValue,
            float speed = SCORE_ANIM_SPEED)
        {
            float progress = 0f;

            int score;

            while (progress < 1f)
            {
                score = (int)Mathf.Lerp(startValue, endValue, progress);
                progress += speed * Time.deltaTime;

                UpdateScoreText(score);
                yield return null;
            }

            UpdateScoreText(endValue);
        }

        private void UpdateScoreText(int score)
        {
            scoreText.text = score.ToString("N0");
        }

        private void OnScoreChanged(object sender, ScoreChangedEventArgs e)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(AnimateScore(e.OldValue, e.NewValue));
        }
    }
}
